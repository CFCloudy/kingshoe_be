﻿using BUS.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Implements;
using DTO.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.Implements
{
    public class ShippingDetailServices : BaseService<ShippingDetail>
    {
        private GenericRepository<ShippingDetail> _da = new GenericRepository<ShippingDetail>();
        private readonly RequestRepository _requestDA = new RequestRepository();
        public override ReturnObject Create(ShippingDetail entity)
        {
            try
            {
                _da.AddDataCommand(entity);
                return new ReturnObject();
            }
            catch (Exception e)
            {
                return new ReturnObject(e.Message);
            }
        }

        public override ReturnObject Delete(int id)
        {
            try
            {
                _da.DeleteDataCommand(GetById(id));
                return new ReturnObject();
            }
            catch (Exception e)
            {
                return new ReturnObject(e.Message);
            }
        }

        public override IQueryable<ShippingDetail> GetAll()
        {
            return _da.GetAllDataQuery();
        }

        public override ShippingDetail GetById(int id)
        {
            return _requestDA.GetOneOrderDetailByID(id);
        }

        public override Tuple<List<ShippingDetail>, int> GetByRequest(string searchKey = "", int currentPage = 1, int rowPerPage = 20)
        {
            currentPage--;
            var skip = currentPage * rowPerPage;
            return _requestDA.GetListOrderDetailsByRequest(searchKey, skip, rowPerPage);
        }

        public override ReturnObject Update(ShippingDetail entity)
        {
            try
            {
                _da.UpdateDataCommand(entity);
                return new ReturnObject();
            }
            catch (Exception e)
            {
                return new ReturnObject(e.Message);
            }
        }
    }
}
