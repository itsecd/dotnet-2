window.onload = function () {
    getOrders();
}

function selectAllProducts() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "../api/Order/" + document.getElementById("orderChoose").value + "/products");
    xhttp.onload = function () {
        var products = JSON.parse(xhttp.responseText);
        var rows = "<tr>" +
            "<td>" + 'ID' + "</td>" +
            "<td>" + 'Name' + "</td>" +
            "<td>" + 'Price' + "</td>" +
            "<td>" + 'Date' + "</td>" +
            "</tr>";
        for (i = 0; i < products.length; i++) {
            rows +=
                "<tr>" +
                "<td>" + products[i].productId + "</td>" +
                "<td>" + products[i].name + "</td>" +
                "<td>" + products[i].price + "</td>" +
                "<td>" + products[i].date + "</td></tr > ";
        }
        document.getElementById("productTable").innerHTML = rows;
    }
    xhttp.send();
}

function getOrders() {
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
        document.getElementById("orderChoose").innerHTML = rows;
        selectAllProducts();
    }
    xhttp.send();
}

function addProduct() {
    let itemToInsert = {
        name: document.getElementById('name').value,
        price: document.getElementById('price').value,
        date: document.getElementById('date').value
    };
    let itemToInsertJson = JSON.stringify(itemToInsert);
    let xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        selectAllProducts();
    }
    xhttp.open("POST", "../api/Order/" + document.getElementById("orderChoose").value + "/products");
    xhttp.setRequestHeader('Content-Type', 'application/json');
    xhttp.send(itemToInsertJson);
}