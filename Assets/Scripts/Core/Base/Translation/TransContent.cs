// ******************************************************************
//       /\ /|       @file       TransContent.cs
//       \ V/        @brief      翻译文本 作为一份独立的翻译内容
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-18 12:53:32
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Text;

namespace Rabi
{
//参数数组args替换掉标记符{0}{1}...可以得到一个string结构的文本
//注意TransContent也可以作为动态参数,避免两个TransContent嵌套递归
    public struct TransContent
    {
        public int transId; //翻译id
        public object[] args; //参数数组 用于替换掉标记符
        private StringBuilder _preArg; //前缀
        private StringBuilder _endArg; //后缀
        private StringBuilder _resultBuilder; //用于拼接最终结果

        public static TransContent Default
        {
            get
            {
                TransContent transContent;
                transContent.transId = 0;
                transContent.args = null;
                transContent._preArg = null;
                transContent._endArg = null;
                transContent._resultBuilder = null;
                return transContent;
            }
        }

        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="value"></param>
        public void AddPreArg(string value)
        {
            _preArg ??= new StringBuilder();
            _preArg.Append(value);
        }

        /// <summary>
        /// 添加后缀
        /// </summary>
        /// <param name="value"></param>
        public void AddEndArg(string value)
        {
            _endArg ??= new StringBuilder();
            _endArg.Append(value);
        }

        public override string ToString()
        {
            _resultBuilder ??= new StringBuilder();
            //添加前缀
            if (_preArg != null)
            {
                _resultBuilder.Append(_preArg);
            }

            var text = LanguageUtil.GetTransStr(transId);
            //存在动态参数
            if (args is { Length: > 0 })
            {
                //替换动态参数
                for (var i = 0; i < args.Length; i++)
                {
                    //当前占位符
                    var placeholder = $"{{{i}}}";
                    text = text.Replace(placeholder, args[i].ToString());
                }
            }

            _resultBuilder.Append(text);
            //添加后缀
            if (_endArg != null)
            {
                _resultBuilder.Append(_endArg);
            }

            return _resultBuilder.ToString();
        }
    }
}