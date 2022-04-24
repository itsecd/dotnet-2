window.onload = function () {
    selectAllItems();
    
}

function selectAllItems() {
	const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Order/');
	xhttp.onload = function() {
        const orders = JSON.parse(xhttp.responseText);
        let rows = '<tr>' +
            '<td>' + 'ID' + '</td>' +
            '<td>' + 'Customer' + '</td>' +
            '<td>' + 'Price' + '</td>' +
            '<td>' + 'Status' + '</td>' +
            '<td>' + 'Date' + '</td>' +
            '<td><table><tr>' + 'Products' + '</tr>' +
            '<tr>' +
            '<td>' + 'ID' + '</td>' +
            '<td>' + 'Name' + '</td>' +
            '<td>' + 'Price' + '</td>' +
            '<td>' + 'Date' + '</td>'+
            '</tr></table></td>' +
            '</tr>';
        for (i = 0; i < orders.length; i++) {
            rows +=
                '<tr>' +
                '<td>' + orders[i].orderId + '</td>' +
                '<td>' + orders[i].customer.name + '</td>' +
                '<td>' + orders[i].price + '</td>' +
                '<td>' + orders[i].status + '</td>' +
                '<td>' + new Date(orders[i].date).toDateString() + '</td>';
            rows += '<td><table>';
            for (j = 0; j < orders[i].products.length; j++) {
                rows +=
                    '<tr>' +
                    '<td>' + orders[i].products[j].productId + '</td>' +
                    '<td>' + orders[i].products[j].name + '</td>' +
                    '<td>' + orders[i].products[j].price + '</td>' +
                '<td>' + new Date(orders[i].products[j].date).toDateString() + '</td></tr > ';
            }
            rows += '</table></td></tr>';
        }
        document.getElementById('orderTable').innerHTML = rows;
    }
	xhttp.send();
}

function changeOrder() {
    if (CheckValidationData('.changeOrder')) {
        const xhttp = new XMLHttpRequest();
        xhttp.open('GET', url +'api/Customer/');
        xhttp.onload = function () {
            let customers = JSON.parse(xhttp.responseText);
            for (let i = 0; i < customers.length; i++) {
                if (customers[i].customerId == document.getElementById('updateCustomerChoose').value) {
                    const CustomerToUpdate = {
                        customerId: customers[i].customerId,
                        name: customers[i].name,
                        phone: customers[i].phoneNumber,
                        address: customers[i].address
                    };
                    const id = document.getElementById('updateOrderChoose').value;

                    const xhttpGetProducts = new XMLHttpRequest();
                    xhttpGetProducts.open('GET', url + 'api/Order/' + id + '/products');
                    xhttpGetProducts.onload = function () {
                        const itemToUpdate = {
                            customer: CustomerToUpdate,
                            status: document.getElementById('updateStatus').value,
                            date: document.getElementById('updateDate').value,
                            products: JSON.parse(xhttpGetProducts.responseText)
                        };
                        const itemToUpdateJson = JSON.stringify(itemToUpdate);
                        const xhttpPut = new XMLHttpRequest();
                        xhttpPut.open('PUT', url + 'api/Order/' + id);
                        xhttpPut.setRequestHeader('Content-Type', 'application/json');
                        xhttpPut.onload = function () {
                            if (xhttpPut.status === 200) {
                                selectAllItems();
                                getOrdersToChange();
                            } else {
                                alert('Order dont change');
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
        const id = document.getElementById('updateOrderStatusChoose').value;
        const xhttp = new XMLHttpRequest();
        xhttp.open('PATCH', url + 'api/Order/' + id);
        const itemToUpdate = {
            status: document.getElementById('statusToUpdate').value
        };
        const itemToUpdateJson = JSON.stringify(itemToUpdate);
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status === 200) {
                selectAllItems();
                getOrdersToChangeStatus();
            } else {
                alert('Order status dont change');
            }
        }
        xhttp.send(itemToUpdateJson);
    }
}

function changeProduct() {
    if (CheckValidationData('.changeProduct')) {
        const idOrder = document.getElementById('updateOrderToProductChoose').value;
        const idProduct = document.getElementById('updateProductChoose').value;
        const xhttp = new XMLHttpRequest();
        xhttp.open('PATCH', url + 'api/Order/' + idOrder + '/products/' + idProduct);
        const itemToUpdate = {
            name: document.getElementById('updateProductName').value,
            price: document.getElementById('updateProductPrice').value,
            date: document.getElementById('updateProductDate').value
        };
        const itemToUpdateJson = JSON.stringify(itemToUpdate);
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.onload = function () {
            if (xhttp.status === 200) {
                selectAllItems();
                getProductsIdToUpdateProduct();
            } else {
                alert('Product dont change');
            }
        }
        xhttp.send(itemToUpdateJson);
    }
}


function deleteOrder() {
    if (CheckValidationData('.deleteOrder')) {
        const id = document.getElementById('deleteChoose').value;
        const xhttp = new XMLHttpRequest();
        xhttp.open('DELETE', url + 'api/Order/' + id);
        xhttp.onload = function () {
            if (xhttp.status === 200) {
                selectAllItems();
                getOrdersToDelete();
            } else {
                alert('Order dont delete');
            }
        }
        xhttp.send();
    }
}

function deleteProduct() {
    if (CheckValidationData('.deleteProduct')) {
        const idOrder = document.getElementById('deleteOrderChoose').value;
        const idProduct = document.getElementById('deleteProductChoose').value;
        const xhttp = new XMLHttpRequest();
        xhttp.open('DELETE', url + 'api/Order/' + idOrder + '/products/' + idProduct);
        xhttp.onload = function () {
            if (xhttp.status === 200) {
                selectAllItems();
                getProductsIdToDeleteProduct();
            } else {
                alert('Product dont delete');
            }

        }
        xhttp.send();
    }
}

function getAllPrice() {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url +  'api/Order/all-price');
    xhttp.onload = function () {
        alert('All Price Orders is ' + xhttp.responseText);
    }
    xhttp.send();
}

function getCountProductMonthly() {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Order/products-monthly');
    xhttp.onload = function () {
        alert('All Count Products By Order Monthly is ' + xhttp.responseText);
    }
    xhttp.send();
}

function getCustomersToChange() {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Customer/');
    xhttp.onload = function() {
        let customers = JSON.parse(xhttp.responseText);
        let rows = '<option value = "">--</option>';
        for (let i = 0; i < customers.length; i++) {
            rows +=
                '<option value = ' + customers[i].customerId + '>' +
                customers[i].name + '</option>';
        }
        document.getElementById('updateCustomerChoose').innerHTML = rows;
    }
    xhttp.send();
}

function getOrdersToChange() {
    getOrders('updateOrderChoose');
}

function getDateToChangeOrder() {
    document.getElementById('updateDate').valueAsDate = new Date();
}

function getStatusToChangeOrder() {
    getStatus('updateStatus');
}

function getOrdersToChangeStatus() {
    getOrders('updateOrderStatusChoose');
}

function getStatusToChangeStatus() {
    getStatus('statusToUpdate');
}

function getOrdersToDelete() {
    getOrders('deleteChoose');
}

function getOrdersToDeleteProduct() {
    getOrders('deleteOrderChoose');    
}

function getProductsIdToDeleteProduct() {
    getProducts('deleteOrderChoose', 'deleteProductChoose');
}

function getOrdersToUpdateProduct() {
    getOrders('updateOrderToProductChoose');
}

function getDateToChangeProduct() {
    document.getElementById('updateProductDate').valueAsDate = new Date();
}


function getProductsIdToUpdateProduct() {
    getProducts('updateOrderToProductChoose', 'updateProductChoose');
}

function getProducts(orderChooseName, productChooseName) {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Order/' + document.getElementById(orderChooseName).value + '/products');
    xhttp.onload = function () {
        const products = JSON.parse(xhttp.responseText);
        let rows = '<option value = "">--</option>';
        for (let i = 0; i < products.length; i++) {
            rows +=
                '<option value = ' + products[i].productId + '>' +
                products[i].productId + '</option>';
        }
        document.getElementById(productChooseName).innerHTML = rows;
    }
    xhttp.send();

}

function getOrders(orderChooseName) {
    const xhttp = new XMLHttpRequest();
    xhttp.open('GET', url + 'api/Order/');
    xhttp.onload = function () {
        const orders = JSON.parse(xhttp.responseText);
        let rows = '<option value = "">--</option>';
        for (let i = 0; i < orders.length; i++) {
            rows +=
                '<option value = ' + orders[i].orderId + '>' +
                orders[i].customer.name + '(' + new Date(orders[i].date).toDateString() + ')' + '</option>';
        }
        document.getElementById(orderChooseName).innerHTML = rows;
    }
    xhttp.send();
}

function getStatus(statusChooseName) {
    let rows = '<option value = "">--</option>' +
        '<option value = "0">0. Initial</option>' +
        '<option value = "1">1. In Process</option>' +
        '<option value = "2">2. Referred</option>' +
        '<option value = "3">3. Partly Serviced</option>' +
        '<option value = "4">4. Quote Finished</option>' +
        '<option value = "5">5. Waiting for Customer</option>' +
        '<option value = "6">6. Spare Part Ordered</option>' +
        '<option value = "7">7. Spare Part Received</option>' +
        '<option value = "8">8. Finished</option>';
    document.getElementById(statusChooseName).innerHTML = rows;
}

function CheckValidationData(divName) {
    let flag = true;

    const form = document.querySelector('.start-page');
    const div = form.querySelector(divName);
    const fields = div.querySelectorAll('.field');
    const errors = div.querySelectorAll('.error');

    for (let i = 0; i < errors.length; i++) {
        errors[i].remove();
    }

    for (let i = 0; i < fields.length; i++) {
        if (!fields[i].value) {
            const error = document.createElement('div');
            error.className = 'error';
            error.style.color = 'red';
            error.innerHTML = 'Cannot be blank';
            fields[i].parentElement.insertBefore(error, fields[i]);
            flag = false;
        }
    }

    return flag;
}


