using UnityEngine;

namespace Slate
{
    ///Defines a section...
    [System.Serializable]
    public class Section
    {
        public enum ExitMode
        {
            Continue,
            Loop,
        }

        ///Default color of Sections
        public static readonly Color DefaultColor = Color.green.WithAlpha(0.3f);

        [SerializeField] private string _UID;
        [SerializeField] private string _name;
        [SerializeField] private float _time;
        [SerializeField] private ExitMode _exitMode;
        [SerializeField] private int _loopCount;
        [SerializeField] private Color _color = DefaultColor;
        [SerializeField] private bool _colorizeBackground;

        ///The current loop iteration if section is looping
        public int CurrentLoopIteration { get; private set; }

        ///Unique ID.
        public string Uid
        {
            get => _UID;
            private set => _UID = value;
        }

        ///The name of the section.
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        ///It's time.
        public float Time
        {
            get => _time;
            set => _time = value;
        }

        ///What will happen when the section has reached it's end?
        public ExitMode exitMode
        {
            get { return _exitMode; }
            set { _exitMode = value; }
        }

        public int LoopCount
        {
            get => _loopCount;
            set => _loopCount = value;
        }

        ///Preferrence color.
        public Color Color
        {
            get => _color.a > 0.1f ? _color : DefaultColor;
            set => _color = value;
        }

        ///Will the timlines bg be colorized as well?
        public bool ColorizeBackground
        {
            get => _colorizeBackground;
            set => _colorizeBackground = value;
        }

        ///A new section of name at time
        public Section(string name, float time)
        {
            this.Name = name;
            this.Time = time;
            Uid = System.Guid.NewGuid().ToString();
        }

        ///Rest the looping state
        public void ResetLoops()
        {
            CurrentLoopIteration = 0;
        }

        ///Breaks out of the loop
        public void BreakLoop()
        {
            CurrentLoopIteration = int.MaxValue;
        }

        ///Updates looping state and returns if looped
        public bool TryUpdateLoop()
        {
            if (LoopCount <= 0 && CurrentLoopIteration != int.MaxValue)
            {
                return true;
            }

            if (CurrentLoopIteration >= LoopCount) return false;
            CurrentLoopIteration++;
            return true;
        }

        public override string ToString()
        {
            return $"'{Name}' Section Time: {Time}";
        }
    }
}