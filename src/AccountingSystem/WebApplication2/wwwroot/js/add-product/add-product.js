window.onload = function () {
    getOrders();
}

function selectAllProducts() {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://localhost:5002/api/Order/" + document.getElementById("orderChoose").value + "/products");
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
                "<td>" + new Date(products[i].date).toDateString() + "</td></tr > ";
        }
        document.getElementById("productTable").innerHTML = rows;
    }
    xhttp.send();
}

function getOrders() {
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
        document.getElementById("orderChoose").innerHTML = rows;
        selectAllProducts();
    }
    xhttp.send();
}

function addProduct() {
    if (CheckValidationData()) {
        let itemToInsert = {
            name: document.getElementById('name').value,
            price: document.getElementById('price').value,
            date: document.getElementById('date').value
        };
        let itemToInsertJson = JSON.stringify(itemToInsert);
        let xhttp = new XMLHttpRequest();
        xhttp.onload = function () {
            if (xhttp.status == 200) {
                selectAllProducts();
            } else {
                alert("Customer don't add");
            }
        }
        xhttp.open("POST", "https://localhost:5002/api/Order/" + document.getElementById("orderChoose").value + "/products");
        xhttp.setRequestHeader('Content-Type', 'application/json');
        xhttp.send(itemToInsertJson);
    }
}

function CheckValidationData() {
    var flag = true;

    var form = document.querySelector('.add-page')
    var fields = form.querySelectorAll('.field')
    var errors = form.querySelectorAll('.error')

    for (var i = 0; i < errors.length; i++) {
        errors[i].remove()
    }

    for (var i = 0; i < fields.length; i++) {
        if (!fields[i].value) {
            var error = document.createElement('div')
            error.className = 'error'
            error.style.color = 'red'
            error.innerHTML = 'Cannot be blank'
            form[i].parentElement.insertBefore(error, fields[i])
            flag = false
        }
    }

    return flag
}