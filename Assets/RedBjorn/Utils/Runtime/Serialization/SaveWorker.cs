namespace RedBjorn.Utils
{
    public abstract class SaveWorker
    {
        ISaveWorkCallback SaverCallback;

        public SaveWorker(ISaveWorkCallback callback)
        {
            SaverCallback = callback;
        }

        public void Save(byte[] save, string savename)
        {
            Log.I($"Saving. Savename: {savename} started");
            DoSave(save, savename);
        }

        public void Load(string savename)
        {
            Log.I($"Loading. Savename: {savename} started");
            DoLoad(savename);
        }

        public void Delete(string savename)
        {
            Log.I($"Deleting. Savename: {savename} started");
            DoDelete(savename);
        }

        protected abstract void DoSave(byte[] data, string savename);
        protected abstract void DoLoad(string savename);
        protected abstract void DoDelete(string savename);

        protected void OnSaveCompleted(bool success, byte[] bytes, string savename)
        {
            Log.I($"Saving. Savename: {savename} completed. Success: {success}");
            if (SaverCallback != null)
            {
                SaverCallback.OnSaveCompleted(success, bytes, savename);
            }
        }

        protected void OnLoadCompleted(bool success, byte[] bytes, string savename)
        {
            Log.I($"Loading. Savename: {savename} completed. Success: {success}");
            if (SaverCallback != null)
            {
                SaverCallback.OnLoadCompleted(success, bytes, savename);
            }
        }

        protected void OnDeleteCompleted(bool success, string savename)
        {
            Log.I($"Deleting. Savename: {savename} completed. Success: {success}");
            if (SaverCallback != null)
            {
                SaverCallback.OnDeleteCompleted(success, savename);
            }
        }
    }
}