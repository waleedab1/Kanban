using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    abstract class DTO
    {
        protected DalController _controller;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected DTO(DalController controller)
        {
            _controller = controller;
        }

        public void Insert(DTO Obj)
        {
           bool b = _controller.Insert(Obj);
           if (b)
                logger.Info("Insert succeeded");
           else
                logger.Error("Insert Failed");
        }

        public void Delete(DTO Obj)
        {
            bool b = ((ColumnDalController)_controller).Delete(Obj);
            if (b)
                logger.Info("Delete succeeded");
            else
                logger.Error("Delete Failed");
        }
        public void DeleteTask(DTO Obj)
        {
            bool b = ((TaskDalController)_controller).Delete(Obj);
            if (b)
                logger.Info("Delete succeeded");
            else
                logger.Error("Delete Failed");
        }

        public void Update(string ID, string AttributeName, object AttributeValue)
        {
            bool b = _controller.Update(ID,AttributeName,AttributeValue);
            if (b)
                logger.Info("Update succeeded");
            else
                logger.Error("Update Failed");
        }

        public void InsertGuest(string Email, string GuestEmail)
        {
            ((BoardDalController)_controller).InsertGuest(Email, GuestEmail);
        }
    }
}