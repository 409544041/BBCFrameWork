// ******************************************************************
//       /\ /|       @file       AssetLoadType.cs
//       \ V/        @brief      资源加载类型
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-20 12:10:35
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum AssetLoadType
    {
        Permanent, //永久加载 不会在切换场景时释放
        Temp //临时加载 会在切换场景时释放
    }
}