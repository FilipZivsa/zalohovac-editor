namespace GradeRegisterAdminApp.Presentation.Components
{
    public abstract class BaseComponent : IComponent
    {
        public abstract bool Selectable { get; }

        private bool _inline;

        protected BaseComponent(bool inline = false)
        {
            _inline = inline;
        }

        public virtual void Render(bool selected)
        {
            Console.Write(_inline ? " " : "\n");
        }
        public virtual void HandleKey(ConsoleKeyInfo keyInfo) 
        { }
    }
}
