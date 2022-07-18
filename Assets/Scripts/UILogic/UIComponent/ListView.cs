// ******************************************************************
//       /\ /|       @file       ListView.cs
//       \ V/        @brief      循环列表UI组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-01-02 12:51:59
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rabi
{
    public class ListView : MonoBehaviour
    {
        //布局方向
        public enum Direction
        {
            Horizontal, //水平
            Vertical, //垂直
        }

        #region FIELDS

        public float spacingX; //x间距
        public float spacingY; //y间距
        public float offsetX; //偏移量x
        public float offsetY; //偏移量y
        public GameObject protoObj; //item原型体
        public Direction direction = Direction.Vertical; //布局方向
        public int rowOrCol = 1; //垂直布局表示列的数量 水平布局表示行的数量
        public int tryDrawItemNum; //准备绘制的item数量
        private Action<GameObject, int> _onEnable; //启用时回调 
        private Action<GameObject> _onDisable; //禁用时回调 
        private GameObject _viewport; //可见范围遮罩节点
        private GameObject _content; //内容节点
        private RectTransform _compRectTransformContent; //内容节点tran组件
        private RectTransform _compRectTransformViewport; //遮罩节点tran组件
        private ScrollRect _scrollRect; //滑动组件
        private float _protoHeight; //原型体高度
        private float _protoWidth; //原型体宽度
        private int _cacheItemNum = -1; //当前数据中item的总数量
        private readonly List<Vector3> _objPosList = new List<Vector3>(); //obj物体位置列表
        private readonly Stack<GameObject> _objPool = new Stack<GameObject>(); //item物体对象池
        private bool _hasInit; //已经初始化完成
        private Action<int> _onLastVisitableItemIndexChanged; //最后一个可见元素的索引变化,按元素中位线算 通行证模块用到
        private int _lastVisitableItemIndex; //最后一个可见元素的索引

        private readonly Dictionary<int, GameObject>
            _mapCurrentObjDict = new Dictionary<int, GameObject>(); //当前使用中的obj物体<索引,物体>

        [SerializeField] private bool needOnLastVisitableItemIndexChanged;

        #endregion

        /// <summary>
        /// 注册item启用回调
        /// </summary>
        /// <param name="onItemEnable"></param>
        public void AddListenerOnItemEnable(Action<GameObject, int> onItemEnable)
        {
            if (onItemEnable == null)
            {
                return;
            }

            _onEnable = onItemEnable;
        }

        /// <summary>
        /// 注册item禁用回调
        /// </summary>
        /// <param name="onItemDisable"></param>
        public void AddListenerOnItemDisable(Action<GameObject> onItemDisable)
        {
            if (onItemDisable == null)
            {
                return;
            }

            _onDisable = onItemDisable;
        }

        /// <summary>
        /// 注册滑动进度改变回调
        /// </summary>
        /// <param name="onValueChanged"></param>
        public void AddListenerOnValueChanged(UnityAction<Vector2> onValueChanged)
        {
            if (onValueChanged == null)
            {
                return;
            }

            _scrollRect.onValueChanged.AddListener(onValueChanged);
        }

        /// <summary>
        /// 注册最后一个可见元素的索引变化回调
        /// </summary>
        /// <param name="onLastVisitableItemIndexChanged"></param>
        public void AddListenerOnLastVisitableItemIndexChanged(Action<int> onLastVisitableItemIndexChanged)
        {
            if (onLastVisitableItemIndexChanged == null)
            {
                return;
            }

            this._onLastVisitableItemIndexChanged = onLastVisitableItemIndexChanged;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="itemNum">item绘制数量</param>
        public void Refresh(int itemNum)
        {
            tryDrawItemNum = itemNum;
            Refresh();
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            if (!_hasInit)
            {
                Init();
            }

            ClearItems();
        }

        /// <summary>
        /// 撤销监听
        /// </summary>
        public void RemoveAllListeners()
        {
            _onEnable = null;
            _onDisable = null;
            _onLastVisitableItemIndexChanged = null;
        }

        /// <summary>
        /// 设置内容节点的位置
        /// </summary>
        /// <param name="index"></param>
        public void SetContentPos(int index)
        {
            if (index > _objPosList.Count || index < 0)
            {
                Debug.LogError("值异常");
                return;
            }

            if (direction == Direction.Horizontal)
            {
                _compRectTransformContent.anchoredPosition3D = new Vector3(-_objPosList[index].x, 0, 0);
                return;
            }

            _compRectTransformContent.anchoredPosition3D = new Vector3(0, -_objPosList[index].y, 0);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh()
        {
            if (!_hasInit)
            {
                Init();
            }

            //原型体校验
            CheckProtoTransform();
            //尝试回收多余的item
            TryRecycleItems(tryDrawItemNum);
            //对剩余的item重新刷新
            ReEnableItemList();
            // 计算content尺寸
            CalcContentSize(tryDrawItemNum);
            //计算储存每个item坐标信息
            CalcEachItemPosition(tryDrawItemNum);
            //尝试放置item
            TrySetItems();
            //记录当前item总数量
            _cacheItemNum = tryDrawItemNum;
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
            tryDrawItemNum = 0;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //滑动组件校验
            CheckScrollRect();
            //内容节点校验
            CheckContentTransform();
            _scrollRect.onValueChanged.AddListener(RebuildLayout);
            if (needOnLastVisitableItemIndexChanged)
            {
                _scrollRect.onValueChanged.AddListener(CheckLastVisitableItemIndex);
            }

            _hasInit = true;
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
        /// 重启Item列表
        /// </summary>
        private void ReEnableItemList()
        {
            foreach (var kvp in _mapCurrentObjDict)
            {
                _onDisable?.Invoke(kvp.Value);
                _onEnable?.Invoke(kvp.Value, kvp.Key);
            }
        }

        /// <summary>
        /// 尝试回收item
        /// </summary>
        private void TryRecycleItem(int i)
        {
            _mapCurrentObjDict.TryGetValue(i, out var obj);
            if (obj == null)
            {
                return;
            }

            //回收物体
            _onDisable?.Invoke(obj);
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
                //超出可见范围
                if (IsOutOfVisitableRange(_objPosList[i]))
                {
                    //回收物体
                    TryRecycleItem(i);
                    continue;
                }

                //没有超出可见范围
                _mapCurrentObjDict.TryGetValue(i, out var obj);
                //物体已经存在
                if (obj != null)
                {
#if UNITY_EDITOR
                    //编辑器模式调整间距时 物体在可视范围内位置可能发生了变化 运行时位置不会变
                    obj.transform.localPosition = _objPosList[i];
#endif
                    continue;
                }

                //物体不存在 尝试从对象池中取出
                obj = PopObj();
                _mapCurrentObjDict.Add(i, obj);
                obj.transform.localPosition = _objPosList[i];
                //启用回调
                _onEnable?.Invoke(obj, i);
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
                        x = (i % rowOrCol) * (_protoWidth + spacingX) + offsetX;
                        //y = 该元素位于第几行 * （原型体高度 + Y间距）+ Y偏移量
                        // ReSharper disable once PossibleLossOfFraction
                        y = (i / rowOrCol) * (_protoHeight + spacingY) + offsetY;
                        break;
                    case Direction.Horizontal:
                        // ReSharper disable once PossibleLossOfFraction
                        x = (_protoWidth + spacingX) * (i / rowOrCol) + offsetX;
                        y = (_protoHeight + spacingY) * (i % rowOrCol) + offsetY;
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
                obj = Instantiate(protoObj, _content.transform);
                //创建的时候根据顺序命名
                obj.name = protoObj.name + _content.transform.childCount;
            }

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
            var rect = compProtoRectTransform.rect;
            _protoHeight = rect.height;
            _protoWidth = rect.width;
            compProtoRectTransform.pivot = new Vector2(0, 1);
            //锚点的情况
            if (compProtoRectTransform.anchorMin == compProtoRectTransform.anchorMax)
            {
                compProtoRectTransform.anchorMin = new Vector2(0, 1);
                compProtoRectTransform.anchorMax = new Vector2(0, 1);
                compProtoRectTransform.anchoredPosition = Vector2.zero;
                return;
            }

            //锚框的情况 自适应的item 运行时再改变锚点 不然无法保存原锚点
            if (!Application.isPlaying) return;
            compProtoRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, _protoWidth);
            compProtoRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, _protoHeight);
            compProtoRectTransform.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// 内容节点transform校验
        /// </summary>
        private void CheckContentTransform()
        {
            _viewport = _scrollRect.viewport.gameObject;
            _compRectTransformViewport =
                _viewport.GetComponent<RectTransform>() ?? throw new Exception("找不到RectTransform组件:" + _viewport.name);
            _content = _scrollRect.content.gameObject;
            _compRectTransformContent = _content.GetComponent<RectTransform>() ??
                                        throw new Exception("找不到RectTransform组件:" + _compRectTransformContent.name);
            _compRectTransformContent.pivot = new Vector2(0, 1);
            _compRectTransformContent.anchorMin = new Vector2(0, 1);
            _compRectTransformContent.anchorMax = new Vector2(0, 1);
        }

        /// <summary>
        /// 滑动组件校验
        /// </summary>
        private void CheckScrollRect()
        {
            _scrollRect = GetComponent<ScrollRect>();
            if (_scrollRect == null)
            {
                gameObject.AddComponent<ScrollRect>();
            }
        }

        /// <summary>
        /// 重绘布局 更新item
        /// </summary>
        /// <param name="value"></param>
        private void RebuildLayout(Vector2 value)
        {
            TrySetItems();
        }

        /// <summary>
        /// 检测最后一个可见元素的索引
        /// </summary>
        private void CheckLastVisitableItemIndex(Vector2 value)
        {
            //垂直情况暂时不需要 需要的时候再算吧
            if (direction == Direction.Vertical)
            {
                return;
            }

            var posContent = _compRectTransformContent.anchoredPosition; //content的坐标
            foreach (var key in _mapCurrentObjDict.Keys)
            {
                var currentX = _objPosList[key].x + posContent.x;
                //不在最后一个可见元素元素范围内
                if (currentX < _compRectTransformViewport.rect.width - spacingX - 3 * _protoWidth / 2 ||
                    currentX > _compRectTransformViewport.rect.width - _protoWidth / 2) continue;
                if (_lastVisitableItemIndex == key)
                {
                    return;
                }

                _lastVisitableItemIndex = key;
                _onLastVisitableItemIndexChanged?.Invoke(key);
                return;
            }
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

            var eachRowOrColNum = Mathf.Ceil((float)itemNum / rowOrCol); //每列/行item的最大数量
            switch (direction)
            {
                //垂直布局
                case Direction.Vertical:
                {
                    var contentHeight =
                        (spacingY + _protoHeight) * eachRowOrColNum +
                        offsetY; //内容布局高 = (原型体高度 + 间隔高度) * 每列的最大item数量 + 垂直偏移
                    var contentWidth =
                        _protoWidth * rowOrCol + (rowOrCol - 1) * spacingX +
                        offsetX; //内容布局宽 = 原型体宽度 * 列数 + (列数 - 1) * 间距X + 水平偏移
                    _compRectTransformContent.sizeDelta = new Vector2(contentWidth, contentHeight);
                    break;
                }
                //水平布局
                case Direction.Horizontal:
                {
                    var contentWidth = (spacingX + _protoWidth) * eachRowOrColNum + offsetX;
                    var contentHeight = _protoHeight * rowOrCol + (rowOrCol - 1) * spacingY + offsetY;
                    _compRectTransformContent.sizeDelta = new Vector2(contentWidth, contentHeight);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 是否超出可见范围
        /// </summary>
        /// <param name="position">item坐标</param>
        /// <returns></returns>
        private bool IsOutOfVisitableRange(Vector3 position)
        {
            var posContent = _compRectTransformContent.anchoredPosition; //content的坐标
            switch (direction)
            {
                //自身偏移+content偏移>原型体高度 顶部越界
                case Direction.Vertical when position.y + posContent.y > _protoHeight:
                    return true;
                //自身偏移+content偏移<遮罩底部边界坐标 底部越界
                case Direction.Vertical when position.y + posContent.y < -_compRectTransformViewport.rect.height:
                    return true;
                case Direction.Vertical:
                    return false;
                //自身偏移+content偏移<原型体宽度 左部越界
                case Direction.Horizontal when position.x + posContent.x < -_protoWidth:
                    return true;
                //自身偏移+content偏移>遮罩宽度 右部越界
                case Direction.Horizontal when position.x + posContent.x > _compRectTransformViewport.rect.width:
                    return true;
                case Direction.Horizontal:
                    return false;
                default:
                    return false;
            }
        }

        #region UNITY LIFE

        private void Awake()
        {
            if (_hasInit)
            {
                return;
            }

            Init();
        }

        private void OnDestroy()
        {
            ClearItems();
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
    
            if (!_hasInit)
            {
                Init();
            }
    
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
            for (var i = _compRectTransformContent.childCount - 1; i >= 0; i--)
            {
                TryRecycleItem(i);
                var compTransformChild = _compRectTransformContent.GetChild(i);
                DestroyImmediate(compTransformChild.gameObject);
            }

            _objPool.Clear();
            _mapCurrentObjDict.Clear();
            tryDrawItemNum = 0;
            _hasInit = false;
        }
    }
}