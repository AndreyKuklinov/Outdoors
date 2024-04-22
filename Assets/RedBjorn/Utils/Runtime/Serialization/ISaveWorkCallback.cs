namespace RedBjorn.Utils
{
    public interface ISaveWorkCallback
    {
        void OnSaveCompleted(bool success, byte[] data, string savename);
        void OnLoadCompleted(bool success, byte[] bytes, string savename);
        void OnDeleteCompleted(bool success, string savename);
    }
}
