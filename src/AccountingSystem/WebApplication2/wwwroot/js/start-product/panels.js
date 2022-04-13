function openChangePanel() {
    document.getElementById("changeDiv").style.display = "block";
}

function closeChangePanel() {
    document.getElementById("updateName").value = "";
    document.getElementById("updatePrice").value = "";
    document.getElementById("updateDate").value = "";
    document.getElementById("changeDiv").style.display = "none";
}

function openDeletePanel() {
    document.getElementById("deleteDiv").style.display = "block";
}

function closeDeletePanel() {
    document.getElementById("deleteDiv").style.display = "none";
}


