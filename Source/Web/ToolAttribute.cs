using System;

namespace SkyChain.Web
{
    /// <summary>
    /// To specify a user interface tool(set) that works in a particular pattern.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ToolAttribute : Attribute
    {
        public static readonly ToolAttribute BUTTON_CONFIRM = new ToolAttribute(Modal.ButtonConfirm);

        public static readonly ToolAttribute BUTTON_PICK_CONFIRM = new ToolAttribute(Modal.ButtonPickConfirm);

        public static readonly ToolAttribute BUTTON_SCRIPT = new ToolAttribute(Modal.ButtonScript);

        public static readonly ToolAttribute BUTTON_PICK_SCRIPT = new ToolAttribute(Modal.ButtonPickScript);

        readonly Modal modal;

        readonly int element; // ui element

        readonly int mode; // ui mode

        readonly int pick; // form value pick

        readonly Appear appear;

        readonly bool access;

        public ToolAttribute(Modal modal, Appear appear = Appear.Full, bool access = true)
        {
            this.modal = modal;
            this.element = (int) modal & 0xf000;
            this.mode = (int) modal & 0x00ff;
            this.pick = (int) modal & 0x0300;
            this.appear = appear;
            this.access = access;
        }

        public Appear Size => appear;

        public bool IsAnchorTag => element == 0x1000;

        public bool IsButtonTag => element == 0x2000;

        public bool IsPost => HasConfirm || HasPrompt || HasShow;

        public bool MustPick => pick == 0x0100;

        public bool HasConfirm => mode == 0x01;

        public bool HasPrompt => mode == 0x02;

        public bool HasShow => mode == 0x04;

        public bool HasOpen => mode == 0x08;

        public bool HasScript => mode == 0x10;

        public bool HasCrop => mode == 0x20;
    }
}