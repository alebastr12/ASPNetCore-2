Cart = {
    _properties:{
        getCartViewLink: "",
        addToCartLink: "",
        removeFromCartLink: ""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);
        Cart.initEvents();
    },
    initEvents: function () {
        $(".add-to-cart").click(Cart.addToCart);
        $(".cart_quantity_delete").click(Cart.removeFromCart);
    },
    addToCart: function (event) {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("addToCart fail"); });
    },
    showToolTip: function (button) {
        button.tooltip({ title: "Добавленно в корзину." }).tooltip("show");
        setTimeout(function () {
            button.tooltip("destroy");
        }, 500);
    },
    refreshCartView: function () {
        var container = $("#cart-container");
        $.get(Cart._properties.getCartViewLink)
            .done(function (result) {
                container.html(result);
            })
            .fail(function () { console.log("refreshCartView fail"); });
    },
    removeFromCart: function () {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");
        $.get(Cart._properties.removeFromCartLink + "/" + id)
            .done(function () {
                button.closest("tr").remove();
                Cart.refreshTotalPrice();
                Cart.refreshCartView();
            })
            .fail(function () { console.log("removeFromCart fail"); });
    },
    refreshTotalPrice: function () {
        var total = 0;

        $(".cart_total_price").each(function () {
            var price = parseFloat($(this).data("price"));
            total += price;
        });

        var value = total.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        $("#total-order-sum").html(value);
    }
}