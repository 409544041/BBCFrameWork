// ******************************************************************
//       /\ /|       @file       Context
//       \ V/        @brief      持有者身上的组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-03 23:20
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public class Context
    {
        public GameObject gameObject;
        public Transform transform;
        public UnitStateController unitStateController;
        public Rigidbody2D rigidbody2D;
        public ThingWithComps self;

        public static Context CreateFromGameObject(ThingWithComps self)
        {
            var context = new Context {gameObject = self.gameObject, self = self};
            context.transform = context.gameObject.transform;
            context.unitStateController =
                FsmManager.Instance.GetFsm<UnitStateController>(self.GetInstanceID().ToString());
            context.rigidbody2D = self.GetComponent<Rigidbody2D>();
            return context;
        }
    }
}