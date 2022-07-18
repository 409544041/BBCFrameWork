// ******************************************************************
//       /\ /|       @file       ThinkState
//       \ V/        @brief      行为节点执行状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-03 23:14
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum ThinkState
    {
        [EnumName("等待开始")] WaitStart,
        [EnumName("运行中")] Running,
        [EnumName("成功")] Success,
        [EnumName("失败")] Failure,
    }
}