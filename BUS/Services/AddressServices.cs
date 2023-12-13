using BUS.IServices;
using DAL.Models;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using DTO.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class AddressServices : IAddressServices
    {
        private readonly IGenericRepository<UserAddress> _repoBookAdress;
        public AddressServices()
        {
            _repoBookAdress=new GenericRepository<UserAddress>();
        }
        public bool CreateAddress(CreateAddressDTO model)
        {
            try
            {
                var address = _repoBookAdress.GetAllDataQuery().FirstOrDefault(a => a.IsDefault == true && model.UserId == a.UserId);

                if (address is not null)
                {
                    address.IsDefault = false;
                    _repoBookAdress.UpdateDataCommand(address);
                }

                var newaddress = new UserAddress()
                {
                    IsDefault = model.IsDefault,
                    AddressDetail = model.AddressDetail,
                    City = model.City,
                    District = model.District,
                    PhoneNumber = model.PhoneNumber,
                    Type = model.Type,
                    UserId = model.UserId,
                    Ward = model.Ward,
                    Name = model.Name,
                    CreatedAtTime = DateTime.Now,
                    ModifiTime = DateTime.Now,
                };
                _repoBookAdress.AddDataCommand(newaddress);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var _lstAddress = _repoBookAdress.GetAllDataQuery().FirstOrDefault(x => x.Id == id);
                if (_lstAddress != null)
                {
                    _repoBookAdress.DeleteDataCommand(_lstAddress);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BookAddressDTO> GetBookAddress(GetBookAdressFilterDTO input)
        {
            try
            {
                var _lstAddress = _repoBookAdress.GetAllDataQuery().Where(x => x.UserId == input.UserId);
                var _lst = _lstAddress.Select(x => new BookAddressDTO()
                {
                    AddressDetail = x.AddressDetail,
                    City = x.City,
                    District = x.District,
                    Id = x.Id,
                    IsDefault = x.IsDefault,
                    PhoneNumber = x.PhoneNumber,
                    Type = x.Type,
                    UserId = x.UserId,
                    Ward = x.Ward,
                    Name = x.Name,
                });
                return _lst.ToList();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public BookAddressDTO GetDetail(int id)
        {
            var _lstAddress = _repoBookAdress.GetAllDataQuery().FirstOrDefault(x => x.Id == id);
            if (_lstAddress == null) return null;
            var _lst = new BookAddressDTO()
            {
                AddressDetail = _lstAddress.AddressDetail,
                City = _lstAddress.City,
                District = _lstAddress.District,
                Id = _lstAddress.Id,
                IsDefault = _lstAddress.IsDefault,
                PhoneNumber = _lstAddress.PhoneNumber,
                Type = _lstAddress.Type,
                UserId = _lstAddress.UserId,
                Ward = _lstAddress.Ward,
                Name = _lstAddress.Name,
            };
            return _lst;
        }

        public UpdateAddressDTO UpdateAdress(UpdateAddressDTO model)
        {
            if (!_repoBookAdress.GetAllDataQuery().Any(x => x.Id == model.Id))
                return null;
            var address = _repoBookAdress.GetAllDataQuery().FirstOrDefault(a => a.IsDefault == true && model.UserId == a.UserId);
            if (address is not null && model.IsDefault == true)
            {
                address.IsDefault = false;
                _repoBookAdress.UpdateDataCommand(address);
            }
            var newad = _repoBookAdress.GetAllDataQuery().FirstOrDefault(a => a.Id == model.Id);
            newad.PhoneNumber = model.PhoneNumber;
            newad.Ward = model.Ward;
            newad.AddressDetail = model.AddressDetail;
            newad.City = model.City;
            newad.District = model.District;
            newad.Name = model.Name;
            newad.Type = model.Type;
            newad.ModifiTime = DateTime.Now;
           
            _repoBookAdress.UpdateDataCommand(newad);
           
            return model;
        }
    }
}
