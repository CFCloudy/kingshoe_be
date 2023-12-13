

function loadVariants() {
    let prId = $('#Products').find(":selected").val();
    $.get("/Orders/getListVariantsByProd", { prodID: prId }, function (data) {
        //debugger;
        document.getElementById("Variants").innerHTML = "";
        var arrOptions = [];
        arrOptions.push("<option disabled selected value=''> Chọn Biến thể </option>")
        for (var i = 0, n = data.length; i < n; i++) {
            if (data[i].productVariantID) {
                arrOptions.push("<option value='" + data[i].productVariantID + "'>" + data[i].productName + "</option>");
            }
        }

        document.getElementById("Variants").innerHTML = arrOptions.join();
    });
}


let listCartItems = [];
let total = 0;
$("button.plus").on("click", function (e) {
    e.preventDefault();
    let varID = $('#Variants').find(":selected").val();
    let prId = $('#Products').find(":selected").val();
    let quant = $('#Quantity').val();
    
    if (varID && quant) {
        var newCard = $('#cardPrototype').clone(true); // clone the card html
        $(newCard).css('display', 'block').removeAttr('id');
        $.get("/Orders/getTotal", { varId: varID, quant: quant }, (data) => {
            debugger;
            let checkE = listCartItems.findIndex(o => o.VariantID == varID && o.ProductId == prId);
            if (checkE < 0) {
                let varName = $('#Variants').find(":selected").text();
                let item = { VariantID: varID, VariantName: varName, ProductId: prId, Quantity: quant, Price: data / quant, SubTotal: data };
                listCartItems.push(item);
                
            } else {
                listCartItems[checkE].Quantity = listCartItems[checkE].Quantity + parseInt(quant);
                listCartItems[checkE].SubTotal = listCartItems[checkE].SubTotal + parseFloat(data);
            }
            total = 0;
            $('#cart').html('<div id="newCardHolder"><label>Giỏ</label></div>');
            $.each(listCartItems,(index,obj) => {
                $(newCard).find('.itemName').val(obj.VariantName);
                $(newCard).find('.itemPrice').val(obj.Price);
                $(newCard).find('.itemTotal').val(obj.SubTotal);
                total += parseFloat(obj.SubTotal);
                $('#newCardHolder').append(newCard);
            });
            $('#Total').val(parseFloat(total));
        });
    }
});



function toggleShipping() {
    let status = $('#Shipping').is(':checked');
    if (status) {
        $('#FormShipping').show();
    } else {
        $('#FormShipping').hide();
    }
}

$("#createOrder").on("click", function (e) {
    e.preventDefault();
    let shippingDetails;
    if ($('#Shipping').is(':checked')) {
        let shipName = $('#shipName').val();
        let shipAddress = $('#shipAddress').val();
        let shipPhone = $('#shipPhone').val();
        let shipNote = $('#shipNote').val();
        shippingDetails = {
            OrderId: 0, ShippingName: shipName, ShippingAddress: shipAddress,
            ShippingPhone: shipPhone, OrderNote: shipNote
        }

    } else {
        shippingDetails = {
        }
    }
    let userID = $('#User').find(':selected').val();
    let userName = $('#User').find(':selected').text();
    let total = $('#Total').val();
    let orderCode = $('#oCode').val();

    $.post('/Orders/Create', { UserID: userID, UserName: userName, Total: total, OrderCode: orderCode, ListItems: listCartItems, ShippingDetails: shippingDetails, isShip: $('#Shipping').is(':checked') });
});