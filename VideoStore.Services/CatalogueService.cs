﻿using System;
using System.Collections.Generic;
using Common;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Entities;
using VideoStore.Business.Components.Interfaces;

namespace VideoStore.Services
{
    public class CatalogueService : ICatalogueService
    {

        private ICatalogueProvider CatalogueProvider
        {
            get
            {
                return ServiceFactory.GetService<ICatalogueProvider>();
            }
        }

        public List<Business.Entities.Media> GetMediaItems(int pOffset, int pCount)
        {
            return CatalogueProvider.GetMediaItems(pOffset, pCount);
        }


        public Media GetMediaById(int pId)
        {
            return CatalogueProvider.GetMediaById(pId);
        }
    }
}
