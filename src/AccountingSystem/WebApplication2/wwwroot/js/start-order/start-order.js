window.onload = function() {
    selectAllItems();
}

function selectAllItems() {
	var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Order/");
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
                "<td>" + new Date(orders[i].date).toDateString() + "</td>";
            rows += "<td><table>"
            for (j = 0; j < orders[i].products.length; j++) {
                rows +=
                    "<tr>" +
                    "<td>" + orders[i].products[j].productId + "</td>" +
                    "<td>" + orders[i].products[j].name + "</td>" +
                    "<td>" + orders[i].products[j].price + "</td>" +
                "<td>" + new Date(orders[i].products[j].date).toDateString() + "</td></tr > ";
            }
            rows += "</table></td></tr>";
        }
        document.getElementById("orderTable").innerHTML = rows;
    }
	xhttp.send();
}

function changeOrder() {
    if (CheckValidationData('.changeOrder')) {
        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", "https://localhost:5002/api/Customer/");
        xhttp.onload = function () {
            var customers = JSON.parse(xhttp.responseText);
            for (let i = 0; i < customers.length; i++) {
                if (customers[i].customerId == document.getElementById('updateCustomerChoose').value) {
                    let CustomerToUpdate = {
                        customerId: customers[i].customerId,
                        name: customers[i].name,
                        phone: customers[i].phoneNumber,
                        address: customers[i].address
                    };
                    var id = document.getElementById('updateOrderChoose').value

                    var xhttpGetProducts = new XMLHttpRequest();
                    xhttpGetProducts.open("GET", "https://localhost:5002/api/Order/" + id + "/products");
                    xhttpGetProducts.onload = function () {
                        var itemToUpdate = {
                            customer: CustomerToUpdate,
                            status: document.getElementById('updateStatus').value,
                            date: document.getElementById('updateDate').value,
                            products: JSON.parse(xhttpGetProducts.responseText)
                        };
                        var itemToUpdateJson = JSON.stringify(itemToUpdate);
                        var xhttpPut = new XMLHttpRequest();
                        xhttpPut.open('PUT', 'https://localhost:5002/api/Order/' + id);
                        xhttpPut.setRequestHeader('Content-Type', 'application/json');
                        xhttpPut.onload = function () {
                            if (xhttpPut.status == 200) {
                                selectAllItems();
                                getOrdersToChange();
                            } else {
                                alert("Order don't change");
                            }
                        }
                        xhttpPut.send(itemToUpdateJson);
                    }
                    xhttpGetProducts.send();
                }
            }
        }
        xhttp.send();
    }
}

function changeOrderStatus() {
    if (CheckValidationData('.changeOrderStatus')) {
        var id = document.getElementById('updateOrderStatusChoose').value
        var xhttp = new XMLHttpRequest();
        xhttp.open("PATCH", "https://localhost:5002/api/Order/" + id);
        var itemToUpdate = {
            status: document.getElementById('statusToUpdate').value
        };
        var itemToUpdateJson = JSON.stringify(itemToUpdate);
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllItems();
                getOrdersToChangeStatus();
            } else {
                alert("Order status don't change");
            }
        }
        xhttp.send(itemToUpdateJson);
    }
}

function changeProduct() {
    if (CheckValidationData('.changeProduct')) {
        var idOrder = document.getElementById('updateOrderToProductChoose').value;
        var idProduct = document.getElementById('updateProductChoose').value;
        var xhttp = new XMLHttpRequest();
        xhttp.open("PATCH", 'https://localhost:5002/api/Order/' + idOrder + '/products/' + idProduct);
        var itemToUpdate = {
            name: document.getElementById('updateProductName').value,
            price: document.getElementById('updateProductPrice').value,
            date: document.getElementById('updateProductDate').value
        };
        var itemToUpdateJson = JSON.stringify(itemToUpdate);
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllItems();
                getProductsIdToUpdateProduct();
            } else {
                alert("Product don't change");
            }
        }
        xhttp.send(itemToUpdateJson);
    }
}


function deleteOrder() {
    if (CheckValidationData('.deleteOrder')) {
        var id = document.getElementById('deleteChoose').value;
        var xhttp = new XMLHttpRequest();
        xhttp.open('DELETE', 'https://localhost:5002/api/Order/' + id);
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllItems();
                getOrdersToDelete();
            } else {
                alert("Order don't delete");
            }
        }
        xhttp.send();
    }
}

function deleteProduct() {
    if (CheckValidationData('.deleteProduct')) {
        var idOrder = document.getElementById('deleteOrderChoose').value;
        var idProduct = document.getElementById('deleteProductChoose').value;
        var xhttp = new XMLHttpRequest();
        xhttp.open('DELETE', 'https://localhost:5002/api/Order/' + idOrder + '/products/' + idProduct);
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllItems();
                getProductsIdToDeleteProduct();
            } else {
                alert("Product don't delete");
            }

        }
        xhttp.send();
    }
}

function getAllPrice() {
    var xhttp = new XMLHttpRequest();
    xhttp.open('GET', 'https://localhost:5002/api/Order/all-price');
    xhttp.onload = function () {
        alert("All Price Orders is " + xhttp.responseText)
    }
    xhttp.send();
}

function getCountProductMonthly() {
    var xhttp = new XMLHttpRequest();
    xhttp.open('GET', 'https://localhost:5002/api/Order/products-monthly');
    xhttp.onload = function () {
        alert("All Count Products By Order Monthly is " + xhttp.responseText)
    }
    xhttp.send();
}

function getCustomersToChange() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Customer/");
    xhttp.onload = function() {
        var customers = JSON.parse(xhttp.responseText);
        let rows = '<option value = "">--</option>';
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
    getOrders("updateOrderChoose");
}


function getOrdersToChangeStatus() {
    getOrders("updateOrderStatusChoose");
}

function getOrdersToDelete() {
    getOrders("deleteChoose");
}

function getOrdersToDeleteProduct() {
    getOrders("deleteOrderChoose");    
}

function getProductsIdToDeleteProduct() {
    getProducts("deleteOrderChoose", "deleteProductChoose");
}

function getOrdersToUpdateProduct() {
    getOrders("updateOrderToProductChoose");
}


function getProductsIdToUpdateProduct() {
    getProducts("updateOrderToProductChoose", "updateProductChoose");
}

function getProducts(orderChooseName, productChooseName) {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Order/" + document.getElementById(orderChooseName).value + "/products");
    xhttp.onload = function () {
        var products = JSON.parse(xhttp.responseText);
        let rows = '<option value = "">--</option>';
        for (let i = 0; i < products.length; i++) {
            rows +=
                "<option value = " + products[i].productId + ">" +
                products[i].productId + "</option>";
        }
        document.getElementById(productChooseName).innerHTML = rows;
    }
    xhttp.send();

}

function getOrders(orderChooseName) {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Order/");
    xhttp.onload = function () {
        var orders = JSON.parse(xhttp.responseText);
        let rows = '<option value = "">--</option>';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                "<option value = " + orders[i].orderId + ">" +
                orders[i].orderId + "</option>";
        }
        document.getElementById(orderChooseName).innerHTML = rows;
    }
    xhttp.send();

}

function CheckValidationData(divName) {
    var flag = true;

    var form = document.querySelector('.start-page')
    var div = form.querySelector(divName)
    var fields = div.querySelectorAll('.field')
    var errors = div.querySelectorAll('.error')

    for (var i = 0; i < errors.length; i++) {
        errors[i].remove()
    }

    for (var i = 0; i < fields.length; i++) {
        if (!fields[i].value) {
            var error = document.createElement('div')
            error.className = 'error'
            error.style.color = 'red'
            error.innerHTML = 'Cannot be blank'
            fields[i].parentElement.insertBefore(error, fields[i])
            flag = false
        }
    }

    return flag
}
