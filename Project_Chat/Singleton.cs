namespace Project_Chat
{
    public abstract class Singleton<T> where T : class, new()
    {
        static protected T s_instance = null;

        static public T Instance { get { return s_instance; } }

        static public T Create()
        {
            if(s_instance == null)
                new T();
            return s_instance;
        }

        static public void Release()
        {
            if(s_instance != null)
            {
                (s_instance as Singleton<T>).release();
                s_instance = null;
            }
        }

        protected Singleton()
        {
            s_instance = this as T;
        }

        protected virtual void release()
        {

        }
    }
}
