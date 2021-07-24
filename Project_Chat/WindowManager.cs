using System;
using System.Windows.Forms;

namespace Project_Chat
{
    public class WindowManager
    {
        public delegate void FormEvent(Form form);

        static private WindowManager s_instance = null;

        private FormEvent m_changeMainFormEvent = null;

        #region Constructor
        public WindowManager()
        {

        }
        #endregion Constructor

        #region Static
        static public void Create()
        {
            if (s_instance == null)
                s_instance = new WindowManager();
        }

        static public void Release()
        {
            if (s_instance != null)
                s_instance.release();
        }

        static public Form OpenWindow(Type type)
        {
            if (s_instance != null)
                return s_instance.openWindow(type);
            return null;
        }

        static public Form OpenWindow<T>() where T : Form
        {
            return OpenWindow(typeof(T));
        }

        static public void AddListener_ChangeMainForm(FormEvent action)
        {
            if (s_instance != null)
                s_instance.addListener_ChangeMainForm(action);
        }

        static public void RemoveListener_ChangeMainForm(FormEvent action)
        {
            if (s_instance != null)
                s_instance.removeListener_ChangeMainForm(action);
        }
        #endregion Static

        private void release()
        {
            s_instance = null;
        }

        private Form openWindow(Type type)
        {
            Form form = Activator.CreateInstance(type) as Form;
            m_changeMainFormEvent(form);
            return form;
        }

        private void addListener_ChangeMainForm(FormEvent action)
        {
            m_changeMainFormEvent += action;
        }

        private void removeListener_ChangeMainForm(FormEvent action)
        {
            m_changeMainFormEvent -= action;
        }
    }
}
