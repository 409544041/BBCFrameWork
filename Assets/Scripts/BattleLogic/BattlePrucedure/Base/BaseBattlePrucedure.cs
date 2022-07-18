namespace Rabi
{
    public abstract class BaseBattlePrucedure : IFsmState
    {
        public virtual void OnEnter(params object[] args)
        {
            Logger.Log($"Enter BattlePrucedure:{GetType()}");
        }

        public virtual void OnExit()
        {
            Logger.Log($"Exit BattlePrucedure:{GetType()}");
        }

        public void OnAfterChange()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnPause()
        {
        }

        public virtual void OnResume()
        {
        }
    }
}