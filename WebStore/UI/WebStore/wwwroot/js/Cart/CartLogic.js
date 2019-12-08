Cart = {
    _properties:{
        getCartViewLink: "",
        addToCartLink: "",
        removeFromCartLink: "",
        decrementLink: "",
        removeAllLink:""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);
        Cart.initEvents();
    },
    initEvents: function () {
        $(".add-to-cart").click(Cart.addToCart);
        $(".cart_quantity_delete").click(Cart.removeFromCart);
        $(".cart_quantity_up").click(Cart.incrementItem);
        $(".cart_quantity_down").click(Cart.decrementItem);
        $(".cart_clear").click(Cart.clearCart);
    },
    clearCart: function (event) {
        event.preventDefault();

        $.get(Cart._properties.removeAllLink + "/")
            .done(function(){
                $(".cart_item").each(function () {
                    var container = $(this);
                    container.remove();
                });
                var total = 0;
                var value = total.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
                $("#total-order-sum").html(value);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("remove all fail"); });
        
    },
    incrementItem: function (event) {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");
        var container = button.closest("tr");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                var count = parseInt($(".cart_quantity_input", container).val());
                $(".cart_quantity_input", container).val(count + 1);
                Cart.refreshPrice(container);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("incrementItem fail"); });
    },
    decrementItem: function (event) {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");
        var container = button.closest("tr");

        $.get(Cart._properties.decrementLink + "/" + id)
            .done(function () {
                var count = parseInt($(".cart_quantity_input", container).val());
                if (count > 1) {
                    $(".cart_quantity_input", container).val(count - 1);
                    Cart.refreshPrice(container);
                } else {
                    container.remove();
                    Cart.refreshTotalPrice();
                }
                Cart.refreshCartView();
            })
            .fail(function () { console.log("decrementItem fail"); });
    },
    refreshPrice: function (container) {
        var quantity = parseInt($(".cart_quantity_input", container).val());
        var price = parseFloat($(".cart_price", container).data("price"));
        var totalPrice = quantity * price;

        var value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        $(".cart_total_price", container).data("price", totalPrice);
        $(".cart_total_price", container).html(value);

        Cart.refreshTotalPrice();
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