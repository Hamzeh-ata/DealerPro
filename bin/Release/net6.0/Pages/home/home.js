$('#searchForProducts').keypress(function (e) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        // Enter key was pressed
        // Do something with inputValue
        e.preventDefault(); // Prevent form submission
        var searchQuery = $(this).val(); // Get search query
        var category = $("#productCategory").val();
        if (searchQuery != "" && category!="category") {
            window.location.href = 'Home/products?Category=' + encodeURIComponent(category) + '&&SearchValue=' + encodeURIComponent(searchQuery); 
        }
        if (searchQuery != "" && category === "category") {
            window.location.href = 'Home/products?SearchValue=' + encodeURIComponent(searchQuery); 
        }
    }
});

$(".categorieCard").click(function () {
    //change mainImg based on clicked card id
    var cardId = $(this).attr("id");
    window.location.href = window.location.href = 'Home/' + encodeURIComponent(cardId); 
 // Redirect to product page with search parameter in URL

});



