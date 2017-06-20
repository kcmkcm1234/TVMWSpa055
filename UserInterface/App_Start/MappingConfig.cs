﻿using UserInterface.Models;
using SPAccounts.DataAccessObject.DTO;

namespace UserInterface.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //domain <===== viewmodel
                //viewmodel =====> domain
                //ReverseMap() makes it possible to map both ways.
 
                config.CreateMap<LoginViewModel, User>().ReverseMap();


              


            });
        }
    }
}