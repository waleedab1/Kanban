using System;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    
        interface PersistedObject<T> where T : DataAccessLayer.DTOs.DTO
        {
        T ToDalObject();

        }
    
    
}
