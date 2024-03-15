 $(document).ready(function () {
        function getAddress() {
            $.ajax({
                url: '@Url.Action("GetAddress", "User")',
                type: 'GET',
                success: function (response) {
                    if (response.success) {
                        $(".indirizzoConsegna").text(response.indirizzoConsegna);
                    } else {
                        alert("Si è verificato un errore durante il recupero dell'indirizzo di consegna.");
                    }
                },
                error: function () {
                    alert("Si è verificato un errore durante la richiesta AJAX.");
                }
            });
        }

        getAddress();

    $('.navbar-collapse').on('shown.bs.collapse', function () {
        getAddress();
        });
    });

