using DTO.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IServices
{
    public interface IAddressServices
    {
        bool CreateAddress(CreateAddressDTO model);
        List<BookAddressDTO> GetBookAddress(GetBookAdressFilterDTO input);
        bool Delete(int id);
        BookAddressDTO GetDetail(int id);
        UpdateAddressDTO UpdateAdress(UpdateAddressDTO model);
    }
}
