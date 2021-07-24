namespace Project_Chat
{
    public abstract class Net<T> : Singleton<T> where T : class, new()
    {
        protected abstract string c_domain_base { get; }
        static public string Domain { get { return s_instance == null ? string.Empty : (s_instance as Net<T>).c_domain_base; } }

        protected Net() : base()
        {

        }
    }
}
