window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", "../api/Order/");
	xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        var rows = "<tr>" +
            "<td>" + 'ID' + "</td>" +
            "<td>" + 'Customer' + "</td>" +
            "<td>" + 'Price' + "</td>" +
            "<td>" + 'Status' + "</td>" +
            "<td>" + 'Date' + "</td>" +
            "<td><table><tr>" + 'Products' + "</tr>" +
            "<tr>" +
            "<td>" + 'ID' + "</td>" +
            "<td>" + 'Name' + "</td>" +
            "<td>" + 'Price' + "</td>" +
            "<td>" + 'Date' + "</td>"+
            "</tr></table></td>" +
            "</tr>";
        for (i = 0; i < orders.length; i++) {
            rows +=
                "<tr>" +
                "<td>" + orders[i].orderId + "</td>" +
                "<td>" + orders[i].customer.name + "</td>" +
                "<td>" + orders[i].price + "</td>" +
                "<td>" + orders[i].status + "</td>" +
                "<td>" + orders[i].date + "</td>";
            rows += "<td><table>"
            for (j = 0; j < orders[i].products.length; j++) {
                rows +=
                    "<tr>" +
                    "<td>" + orders[i].products[j].productId + "</td>" +
                    "<td>" + orders[i].products[j].name + "</td>" +
                    "<td>" + orders[i].products[j].price + "</td>" +
                "<td>" + orders[i].products[j].date + "</td></tr > ";
            }
            rows += "</table></td></tr>";
        }
        document.getElementById("orderTable").innerHTML = rows;
    }
	xhttp.send();
}

function changeOrder() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Customer/");
    xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        for (let i = 0; i < customers.length; i++) {
            if(customers[i].customerId == document.getElementById('updateCustomerChoose').value){
                let CustomerToUpdate = {
                    customerId: customers[i].customerId,
                    name: customers[i].name,
                    phone: customers[i].phoneNumber,
                    address: customers[i].address
                };
                var id = document.getElementById('updateOrderChoose').value

                var xhttpGetProducts = new XMLHttpRequest();
                xhttpGetProducts.open("GET", "../api/Order/" + id + "/products");
                xhttpGetProducts.onload = function () {
                    var itemToUpdate = {
                        customer: CustomerToUpdate,
                        status: document.getElementById('updateStatus').value,
                        date: document.getElementById('updateDate').value,
                        products: JSON.parse(xhttpGetProducts.responseText)
                    };
                    var itemToUpdateJson = JSON.stringify(itemToUpdate);
                    var xhttpPut = new XMLHttpRequest();
                    xhttpPut.open('PUT', '../api/Order/' + id);
                    xhttpPut.setRequestHeader('Content-Type', 'application/json');
                    xhttpPut.onload = function () {
                        selectAllItems();
                        getOrdersToChange();
                    }
                    xhttpPut.send(itemToUpdateJson);
                }
                xhttpGetProducts.send();
            }
        }
    }
    xhttp.send();
}

function changeOrderStatus() {
    var id = document.getElementById('updateOrderStatusChoose').value
    var xhttp = new XMLHttpRequest();
    xhttp.open("PATCH", "../api/Order/" + id);
    var itemToUpdate = {
        status: document.getElementById('statusToUpdate').value
    };
    var itemToUpdateJson = JSON.stringify(itemToUpdate);
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.onload = function () {
        selectAllItems();
        getOrdersToChangeStatus();
    }
    xhttp.send(itemToUpdateJson);
}


function deleteOrder() {
    var id = document.getElementById('deleteChoose').value;
    var xhttp = new XMLHttpRequest();
    xhttp.open('DELETE', '../api/Order/' + id);
    xhttp.onload = function() {
        selectAllItems();
        getOrdersToDelete();
    }
    xhttp.send();
}

function deleteProduct() {
    var idOrder = document.getElementById('deleteOrderChoose').value;
    var idProduct = document.getElementById('deleteProductChoose').value;
    var xhttp = new XMLHttpRequest();
    xhttp.open('DELETE', '../api/Order/' + idOrder + '/products/' + idProduct);
    xhttp.onload = function () {
        selectAllItems();
        getProductsIDToDeleteProduct();
    }
    xhttp.send();
}

function getAllPrice() {
    var xhttp = new XMLHttpRequest();
    xhttp.open('GET', '../api/Order/all-price');
    xhttp.onload = function () {
        alert("All Price Orders is " + xhttp.responseText)
    }
    xhttp.send();
}

function getCountProductMonthly() {
    var xhttp = new XMLHttpRequest();
    xhttp.open('GET', '../api/Order/products-monthly');
    xhttp.onload = function () {
        alert("All Count Products By Order Monthly is " + xhttp.responseText)
    }
    xhttp.send();
}

function getCustomersToChange() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Customer/");
    xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < customers.length; i++) {
            rows +=
                "<option value = " + customers[i].customerId + ">" +
                customers[i].name + "</option>";
        }
        document.getElementById("updateCustomerChoose").innerHTML = rows;
    }
    xhttp.send();
}

function getOrdersToChange() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/");
    xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderId + ">" +
                orders[i].orderId + "</option>";
        }
        document.getElementById("updateOrderChoose").innerHTML = rows;
    }
    xhttp.send();
}


function getOrdersToChangeStatus() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/");
    xhttp.onload = function () {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderId + ">" +
                orders[i].orderId + "</option>";
        }
        document.getElementById("updateOrderStatusChoose").innerHTML = rows;
    }
    xhttp.send();
}

function getOrdersToDelete() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/");
    xhttp.onload = function() {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderId + ">" +
                orders[i].orderId + "</option>";
        }
        document.getElementById("deleteChoose").innerHTML = rows;
    }
    xhttp.send();
}

function getOrdersToDeleteProduct() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/");
    xhttp.onload = function () {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '<option>--</option>';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderId + ">" +
                orders[i].orderId + "</option>";
        }
        document.getElementById("deleteOrderChoose").innerHTML = rows;
    }
    xhttp.send();
    
}

function getProductsIDToDeleteProduct() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/" + document.getElementById("deleteOrderChoose").value + "/products");
    xhttp.onload = function () {
        var products = JSON.parse(xhttp.responseText);
        let rows = '';
        for (let i = 0; i < products.length; i++) {
            rows +=
                "<option value = " + products[i].productId + ">" +
                products[i].productId + "</option>";
        }
        document.getElementById("deleteProductChoose").innerHTML = rows;
    }
    xhttp.send();

}