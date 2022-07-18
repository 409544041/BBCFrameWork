// ******************************************************************
//       /\ /|       @file       MoveStaticListView.cs
//       \ V/        @brief      可移动静态列表
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-02-08 16:52:43
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Rabi
{
    public class MoveStaticListView : MonoBehaviour
    {
        //布局方向
        public enum Direction
        {
            Horizontal, //水平
            Vertical, //垂直
        }

        private const int RowOrCol = 1; //垂直布局表示列的数量 水平布局表示行的数量

        #region FIELDS

        public GameObject protoObj; //item原型体
        public GameObject movePosTarget; //目标位置物体
        public float spacingX; //x间距
        public float spacingY; //y间距
        public float offsetX; //偏移量x
        public float offsetY; //偏移量y
        public float expandSpeedEveryFrame; //扩展速度 每帧百分比
        public float protoWidthExpand; //扩展后的宽度
        public Direction direction = Direction.Vertical; //布局方向
        private int _itemCount; //准备绘制的item数量
        private RectTransform _compRectTransformContent; //内容节点tran组件
        private int _selectedIndex = -1; //当前选中的item的索引
        private float _protoHeight; //原型体高度
        private float _protoWidth; //原型体宽度 
        private List<Vector3> _objPosList = new List<Vector3>(); //obj物体位置列表

        private Dictionary<int, GameObject>
            _mapCurrentObjDict = new Dictionary<int, GameObject>(); //当前使用中的obj物体<索引,物体>

        private Stack<GameObject> _objPool = new Stack<GameObject>(); //item物体对象池
        private bool _isMoved; //移动过 还未还原

        #endregion

        public int editorTryDrawItemCount; //编辑器尝试绘制item数量
        public int editorTrySelectIndex; //尝试选中的索引

        #region 事件

        private Action<GameObject, int> _onRefresh; //刷新时回调 <要刷新的item,item的索引>
        private Action<GameObject, int, bool> _onBeforeExpand; //扩展前回调<item物体,当前选中物体的索引,是否是起始移动>
        private Action<GameObject, int, bool> _onAfterExpand; //扩展完成回调<item物体,当前选中物体的索引,是否是起始移动>

        #endregion

        #region 缓存

        private int _cacheItemNum = -1; //当前数据中item的总数量
        private Vector2 _cachePivot; //缓存 计算前的重心
        private Vector2 _cacheAnchorMin; //缓存 计算前的最小锚点
        private Vector2 _cacheAnchorMax; //缓存 计算前的最大锚点
        private Vector3 _cacheAnchoredPosition3D; //缓存 计算前的UI坐标
        private Vector3 _cacheOriginPos; //缓存 扩展前的位置

        #endregion


        /// <summary>
        /// 注册item刷新
        /// </summary>
        /// <param name="onItemRefresh"></param>
        public void AddListenerOnItemRefresh(Action<GameObject, int> onItemRefresh)
        {
            if (onItemRefresh == null)
            {
                return;
            }

            _onRefresh = onItemRefresh;
        }

        /// <summary>
        /// 注册扩展前回调
        /// </summary>
        /// <param name="onBeforeExpand"></param>
        public void AddListenerOnBeforeExpand(Action<GameObject, int, bool> onBeforeExpand)
        {
            if (onBeforeExpand == null)
            {
                return;
            }

            this._onBeforeExpand = onBeforeExpand;
        }

        /// <summary>
        /// 注册扩展后回调
        /// </summary>
        /// <param name="onAfterExpand"></param>
        public void AddListenerOnAfterExpand(Action<GameObject, int, bool> onAfterExpand)
        {
            if (onAfterExpand == null)
            {
                return;
            }

            this._onAfterExpand = onAfterExpand;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="itemNum">item绘制数量</param>
        public void Refresh(int itemNum)
        {
            _itemCount = itemNum;
            Refresh();
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            ClearItems();
        }

        /// <summary>
        /// 撤销监听
        /// </summary>
        public void RemoveAllListeners()
        {
            _onRefresh = null; //刷新时回调 <要刷新的item,item的索引>
            _onBeforeExpand = null; //扩展前回调<item物体,当前选中物体的索引>
            _onAfterExpand = null; //扩展完成回调<item物体,当前选中物体的索引>
        }

        /// <summary>
        /// 移动到最前
        /// </summary>
        public void MoveToFirst(int index)
        {
            _selectedIndex = index;
            StopCoroutine(nameof(CoMoveToFirst));
            StartCoroutine(CoMoveToFirst());
        }

        /// <summary>
        /// 还原位置
        /// </summary>
        public void MoveToOrigin()
        {
            StopCoroutine(nameof(CoMoveToOrigin));
            StartCoroutine(CoMoveToOrigin());
        }

        /// <summary>
        /// 获取当前选中的索引
        /// </summary>
        /// <returns></returns>
        public int GetSelectedIndex()
        {
            return _selectedIndex;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh()
        {
            CheckIfMoved();
            //缓存计算前的节点transform信息
            CacheTransform();
            //内容节点校验
            CheckContentTransform();
            //尝试回收多余的item
            TryRecycleItems(_itemCount);
            // 计算content尺寸
            CalcContentSize(_itemCount);
            //计算储存每个item坐标信息
            CalcEachItemPosition(_itemCount);
            //尝试放置item
            TrySetItems();
            //记录当前item总数量
            _cacheItemNum = _itemCount;
            //还原计算前的节点transform信息
            RevertTransform();
        }

        /// <summary>
        /// 清空所有item
        /// </summary>
        private void ClearItems()
        {
            for (var i = _compRectTransformContent.childCount - 1; i >= 0; i--)
            {
                TryRecycleItem(i);
                var compTransformChild = _compRectTransformContent.GetChild(i);
                Destroy(compTransformChild.gameObject);
            }

            _objPool.Clear();
            _mapCurrentObjDict.Clear();
            _itemCount = 0;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //原型体校验
            CheckProtoTransform();
            _compRectTransformContent = GetComponent<RectTransform>()
                                        ?? throw new Exception("找不到RectTransform组件:" + _compRectTransformContent.name);
        }

        /// <summary>
        /// 尝试回收多余item
        /// </summary>
        /// <param name="itemNum"></param>
        private void TryRecycleItems(int itemNum)
        {
            //当前需要绘制的item总数大于等于缓存中的item总数
            if (itemNum >= _cacheItemNum)
            {
                return;
            }

            //回收多余item
            for (var i = itemNum; i < _cacheItemNum; i++)
            {
                TryRecycleItem(i);
            }
        }

        /// <summary>
        /// 尝试回收item
        /// </summary>
        private void TryRecycleItem(int i)
        {
            _mapCurrentObjDict.TryGetValue(i, out var obj);
            //不存在对应索引的物体
            if (obj == null)
            {
                return;
            }

            PushObj(obj);
            _mapCurrentObjDict.Remove(i);
        }

        /// <summary>
        /// 尝试放置item
        /// </summary>  
        private void TrySetItems()
        {
            for (var i = 0; i < _objPosList.Count; i++)
            {
                _mapCurrentObjDict.TryGetValue(i, out var obj);
                //物体不存在
                if (obj == null)
                {
                    obj = PopObj();
                    _mapCurrentObjDict.Add(i, obj);
                }

                //需要刷新数据
                _onRefresh?.Invoke(obj, i);
                obj.transform.localPosition = _objPosList[i];
                obj.SetActive(true);
                var compRectTransform = obj.GetComponent<RectTransform>();
                compRectTransform.sizeDelta = new Vector2(_protoWidth, _protoHeight);
            }
        }

        /// <summary>
        /// 计算储存每个item坐标信息
        /// </summary>
        /// <param name="itemNum"></param>
        private void CalcEachItemPosition(int itemNum)
        {
            if (itemNum < 0)
            {
                return;
            }

            //清空数据
            _objPosList.Clear();
            for (var i = 0; i < itemNum; i++)
            {
                float x, y;
                switch (direction)
                {
                    //垂直布局情况
                    case Direction.Vertical:
                        //x = 该元素位于第几列 * （原型体宽度 + X间距) + X偏移量
                        x = (i % RowOrCol) * (_protoWidth + spacingX) + offsetX;
                        //y = 该元素位于第几行 * （原型体高度 + Y间距）+ Y偏移量
                        // ReSharper disable once PossibleLossOfFraction
                        y = (i / RowOrCol) * (_protoHeight + spacingY) + offsetY;
                        break;
                    case Direction.Horizontal:
                        // ReSharper disable once PossibleLossOfFraction
                        x = (_protoWidth + spacingX) * (i / RowOrCol) + offsetX;
                        y = (_protoHeight + spacingY) * (i % RowOrCol) + offsetY;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _objPosList.Add(new Vector3(x, -y, 0));
            }
        }

        /// <summary>
        /// obj出栈
        /// </summary>
        /// <returns></returns>
        private GameObject PopObj()
        {
            GameObject obj;
            //尝试从对象池取obj 没有的话创建新实例
            if (_objPool.Count > 0)
            {
                obj = _objPool.Pop();
            }
            else
            {
                obj = Instantiate(protoObj);
                //创建的时候根据顺序命名
                obj.name = protoObj.name + transform.childCount;
            }

            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// obj压入对象池栈
        /// </summary>
        /// <param name="obj"></param>
        private void PushObj(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            _objPool.Push(obj);
            obj.SetActive(false);
        }

        /// <summary>
        /// 原型体transform校验
        /// </summary>
        private void CheckProtoTransform()
        {
            //原型体校验
            if (protoObj == null)
            {
                Logger.Error("找不到原型体");
                return;
            }

            var compProtoRectTransform = protoObj.GetComponent<RectTransform>();
            compProtoRectTransform.pivot = new Vector2(0, 1);
            compProtoRectTransform.anchorMin = new Vector2(0, 1);
            compProtoRectTransform.anchorMax = new Vector2(0, 1);
            compProtoRectTransform.anchoredPosition = Vector2.zero;
            var rect = compProtoRectTransform.rect;
            _protoHeight = rect.height;
            _protoWidth = rect.width;
        }

        /// <summary>
        /// 内容节点transform校验
        /// </summary>
        private void CheckContentTransform()
        {
            _compRectTransformContent.pivot = new Vector2(0, 1);
            _compRectTransformContent.anchorMin = new Vector2(0, 1);
            _compRectTransformContent.anchorMax = new Vector2(0, 1);
        }

        /// <summary>
        /// 计算content尺寸
        /// </summary>
        private void CalcContentSize(int itemNum)
        {
            if (itemNum < 0)
            {
                Logger.Error("itemNum不能小于0");
                return;
            }

            var eachRowOrColNum = Mathf.Ceil((float)itemNum / RowOrCol); //每列/行item的最大数量
            switch (direction)
            {
                //垂直布局
                case Direction.Vertical:
                {
                    var contentHeight =
                        (spacingY + _protoHeight) * eachRowOrColNum +
                        offsetY; //内容布局高 = (原型体高度 + 间隔高度) * 每列的最大item数量 + 垂直偏移
                    var contentWidth =
                        _protoWidth * RowOrCol + (RowOrCol - 1) * spacingX +
                        offsetX; //内容布局宽 = 原型体宽度 * 列数 + (列数 - 1) * 间距X + 水平偏移
                    _compRectTransformContent.sizeDelta = new Vector2(contentWidth, contentHeight);
                    break;
                }
                //水平布局
                case Direction.Horizontal:
                {
                    var contentWidth = (spacingX + _protoWidth) * eachRowOrColNum;
                    var contentHeight = _protoHeight * RowOrCol + (RowOrCol - 1) * spacingY;
                    _compRectTransformContent.sizeDelta = new Vector2(contentWidth, contentHeight);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 检测是否移动过未还原
        /// </summary>
        private void CheckIfMoved()
        {
            if (!_isMoved)
            {
                return;
            }

            MoveToOriginNow();
        }

        /// <summary>
        /// 缓存内容节点transform信息
        /// </summary>
        private void CacheTransform()
        {
            _cachePivot = _compRectTransformContent.pivot;
            _cacheAnchorMax = _compRectTransformContent.anchorMax;
            _cacheAnchorMin = _compRectTransformContent.anchorMin;
            _cacheAnchoredPosition3D = _compRectTransformContent.anchoredPosition3D;
        }

        /// <summary>
        /// 还原transform 计算的坐标的时候私自修改了参数 算完了改回去
        /// </summary>
        private void RevertTransform()
        {
            _compRectTransformContent.anchoredPosition3D = _cacheAnchoredPosition3D;
            _compRectTransformContent.anchorMin = _cacheAnchorMin;
            _compRectTransformContent.anchorMax = _cacheAnchorMax;
            _compRectTransformContent.pivot = _cachePivot;
        }

        /// <summary>
        /// 设置全部item可见性
        /// </summary>
        private void SetAllItemVisitable(bool isVisitable)
        {
            foreach (var pair in _mapCurrentObjDict)
            {
                pair.Value.SetActive(isVisitable);
            }
        }

        /// <summary>
        /// 移动到第一项的位置展示
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoMoveToFirst()
        {
            //只显示选中项
            SetAllItemVisitable(false);
            if (!_mapCurrentObjDict.TryGetValue(_selectedIndex, out var selectedObj))
            {
                yield break;
            }

            selectedObj.SetActive(true);
            var compRectTransformSelected = selectedObj.GetComponent<RectTransform>();
            _cacheOriginPos = selectedObj.transform.position; //缓存位置
            //目标位置
            var targetPosition = movePosTarget.transform.position;
            var progress = 0f;
            _onBeforeExpand?.Invoke(selectedObj, _selectedIndex, true);
            //一边移动一边变形
            while (progress < 1f)
            {
                selectedObj.transform.position = Vector3.Lerp(selectedObj.transform.position, targetPosition, progress);
                compRectTransformSelected.sizeDelta = new Vector2(Mathf.Lerp(_protoWidth, protoWidthExpand, progress),
                    compRectTransformSelected.sizeDelta.y);
                progress += expandSpeedEveryFrame;
                yield return null;
            }

            _onAfterExpand?.Invoke(selectedObj, _selectedIndex, true);
            compRectTransformSelected.sizeDelta = new Vector2(protoWidthExpand, compRectTransformSelected.sizeDelta.y);
            selectedObj.transform.position = targetPosition;
            _isMoved = true;
        }

        /// <summary>
        /// 移动还原
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoMoveToOrigin()
        {
            if (!_mapCurrentObjDict.TryGetValue(_selectedIndex, out var selectedObj))
            {
                yield break;
            }

            var compRectTransformSelected = selectedObj.GetComponent<RectTransform>();
            //目标位置
            var targetPosition = _cacheOriginPos;
            var progress = 0f;
            _onBeforeExpand?.Invoke(selectedObj, _selectedIndex, false);
            //一边移动一边变形
            while (progress < 1f)
            {
                selectedObj.transform.position = Vector3.Lerp(selectedObj.transform.position, targetPosition, progress);
                compRectTransformSelected.sizeDelta = new Vector2(Mathf.Lerp(protoWidthExpand, _protoWidth, progress),
                    compRectTransformSelected.sizeDelta.y);
                progress += expandSpeedEveryFrame;
                yield return null;
            }

            SetAllItemVisitable(true);
            _onAfterExpand?.Invoke(selectedObj, _selectedIndex, false);
            compRectTransformSelected.sizeDelta = new Vector2(_protoWidth, compRectTransformSelected.sizeDelta.y);
            selectedObj.transform.position = targetPosition;
            //撤销选中
            _selectedIndex = -1;
            _isMoved = false;
        }

        /// <summary>
        /// 立刻还原位置
        /// </summary>
        private void MoveToOriginNow()
        {
            if (!_mapCurrentObjDict.TryGetValue(_selectedIndex, out var selectedObj))
            {
                return;
            }

            var compRectTransformSelected = selectedObj.GetComponent<RectTransform>();
            //目标位置
            var targetPosition = _cacheOriginPos;
            _onBeforeExpand?.Invoke(selectedObj, _selectedIndex, false);
            SetAllItemVisitable(true);
            _onAfterExpand?.Invoke(selectedObj, _selectedIndex, false);
            compRectTransformSelected.sizeDelta = new Vector2(_protoWidth, compRectTransformSelected.sizeDelta.y);
            selectedObj.transform.position = targetPosition;
            //撤销选中
            _selectedIndex = -1;
            _isMoved = false;
        }

        /// <summary>   
        /// 释放内存
        /// </summary>
        private void Dispose()
        {
            RemoveAllListeners();
            _objPosList = null;
            _objPool = null;
            _mapCurrentObjDict = null;
            protoObj = null;
            _compRectTransformContent = null;
        }

        #region UNITY LIFE

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            ClearItems();
            Dispose();
        }

#if UNITY_EDITOR
        /// <summary>
        /// inspector值发生改变回调
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
    
            if (_mapCurrentObjDict.Count == 0)
            {
                return;
            }
    
            Init();
            Refresh();
        }
#endif

        #endregion

        /// <summary>
        /// 编辑器模式清空item
        /// </summary>
        [UsedImplicitly]
        private void EditorClearItems()
        {
            Init();
            for (var i = _compRectTransformContent.childCount - 1; i >= 0; i--)
            {
                TryRecycleItem(i);
                var compTransformChild = _compRectTransformContent.GetChild(i);
                DestroyImmediate(compTransformChild.gameObject);
            }

            _objPool.Clear();
            _mapCurrentObjDict.Clear();
        }

        /// <summary>
        /// 绘制item
        /// </summary>
        [UsedImplicitly]
        private void EditorDrawItems()
        {
            Init();
            //缓存计算前的节点transform信息
            CacheTransform();
            //内容节点校验
            CheckContentTransform();
            //回收item
            TryRecycleItems(editorTryDrawItemCount);
            // 计算content尺寸
            CalcContentSize(editorTryDrawItemCount);
            //计算储存每个item坐标信息
            CalcEachItemPosition(editorTryDrawItemCount);
            //尝试放置item
            TrySetItems();
            //记录当前item总数量
            _cacheItemNum = editorTryDrawItemCount;
            //还原计算前的节点transform信息
            RevertTransform();
            _itemCount = editorTryDrawItemCount;
        }

        [UsedImplicitly]
        private void EditorMoveToFirst()
        {
            Init();
            MoveToFirst(editorTrySelectIndex);
        }

        [UsedImplicitly]
        private void EditorMoveToOrigin()
        {
            Init();
            _selectedIndex = editorTrySelectIndex;
            MoveToOrigin();
        }
    }
}