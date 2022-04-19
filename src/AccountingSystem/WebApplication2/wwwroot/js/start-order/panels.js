function openChangeOrderPanel() {
    document.getElementById("changeOrder").style.display = "block";
}

function closeChangeOrderPanel() {
    document.getElementById("updateDate").value = "";
    document.getElementById("updateStatus").value = "";
    document.getElementById("changeOrder").style.display = 'none';
}

function openChangeOrderStatusPanel() {
    document.getElementById("changeOrderStatus").style.display = "block";
}

function closeChangeOrderStatusPanel() {
    document.getElementById("statusToUpdate").value = "";
    document.getElementById("changeOrderStatus").style.display = 'none';
}

function openDeleteOrderPanel() {
    document.getElementById("deleteOrder").style.display = "block";
}

function closeDeleteOrderPanel() {
    document.getElementById("deleteOrder").style.display = "none";
}

function openDeleteProductPanel() {
    document.getElementById("deleteProduct").style.display = "block";
}

function closeDeleteProductPanel() {
    document.getElementById("deleteProduct").style.display = "none";
}

