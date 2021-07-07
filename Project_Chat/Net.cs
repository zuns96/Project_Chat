namespace Project_Chat
{
    public abstract class Net<T> where T : class, new()
    {
        static protected T s_instance = null;

        protected bool isWait = false;
        protected abstract string c_domain_base { get; }

        static public void Create()
        {
            if (s_instance == null)
                s_instance = new T();
        }

        static public void Release()
        {
            if (s_instance != null)
                s_instance = null;
        }
    }
}
