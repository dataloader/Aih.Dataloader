using System;
using System.Collections.Generic;
using Aih.DataLoader.Interfaces;
using Aih.DataLoader.Models;
using Aih.DataLoader.Exceptions;


namespace Aih.DataLoader
{

    public abstract class BaseDataLoader
    {

        protected Dictionary<string, string> _config;
        protected IStatusHandler _statusHandler;
        protected LoaderContext _cntx;

        private string _guid;
        private BatchStatus _status;

        public BaseDataLoader()
        {

        }

        public void InitializeHandlers(ILoaderConfigHandler configHandler, IStatusHandler statusHandler)
        {
            _guid = Guid.NewGuid().ToString();

            //Note: This implementation form is sub-optimal in terms of performance.  Perhaps it is better to find another way to to this
            string dllName = this.GetType().Assembly.GetName().Name; //System.Reflection.Assembly.GetExecutingAssembly().FullName;

            string typeName = this.GetType().Name;
            _config = configHandler.GetLoaderConfig(dllName, typeName);
            _statusHandler = statusHandler;
        }



        public void SetStatusComment(string comment)
        {
            if(_status != null)
            {
                _status.Comment = comment;
                _statusHandler.UpdateBatchStatusRecord(_status);
            }

        }


        public abstract string Initialize();
        public abstract void LoadData();
        public abstract void TransformData();
        public abstract void SaveData();
        public abstract void CleanUp();



        public void RunDataLoader()
        {
            string currentClassName = this.GetType().Name;
            string batchrefrence = Initialize();
            

             _status = new BatchStatus() { BatchName = currentClassName, BatchId = _guid, BatchRefrence = batchrefrence, StartTime = DateTime.Now, Comment = "", Status = "Started" };
            _statusHandler.CreateBatchStatusRecord(_status);

            try
            {
                SetStatusLoad(_status);
                LoadData();

                SetStatusTransform(_status);
                TransformData();

                SetStatusSaving(_status);
                SaveData();

                SetStatusCleanUp(_status);
                CleanUp();

                SetStatusFinished(_status);
            }
            catch (DataLoaderException dx)
            {
                _status.Status = dx.Status;
                _status.Comment = dx.Message;
                _status.FinishTime = DateTime.Now;
                _statusHandler.UpdateBatchStatusRecord(_status);
            }
            catch (Exception ex)
            {
                _status.FinishTime = DateTime.Now;
                _status.Status = "Failed";
                _status.Comment = ex.Message;
                _statusHandler.UpdateBatchStatusRecord(_status);
            }
        }


        private void SetStatusFinished(BatchStatus status)
        {
            status.FinishTime = DateTime.Now;
            status.Status = "Finished";
            _statusHandler.UpdateBatchStatusRecord(status);
        }

        private void SetStatusCleanUp(BatchStatus status)
        {
            status.StartCleanupTime = DateTime.Now;
            status.Status = "Cleaning up";
            _statusHandler.UpdateBatchStatusRecord(status);
        }


        private void SetStatusSaving(BatchStatus status)
        {
            status.StartSaveTime = DateTime.Now;
            status.Status = "Saving";
            _statusHandler.UpdateBatchStatusRecord(status);
        }


        private void SetStatusTransform(BatchStatus status)
        {
            status.StartTransformTime = DateTime.Now;
            status.Status = "Transforming";
            _statusHandler.UpdateBatchStatusRecord(status);
        }

        private void SetStatusLoad(BatchStatus status)
        {
            status.StartLoadTime = DateTime.Now;
            status.Status = "Loading";
            _statusHandler.UpdateBatchStatusRecord(status);
        }


    }
}
