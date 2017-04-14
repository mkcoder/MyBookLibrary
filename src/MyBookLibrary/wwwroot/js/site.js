(function($) {
    $('body').on('click',
        '.js-search-query',
        function(e) {
            let query = $('.js-seach-query-input').val();
            let startIndex = $(this).data('start_index');
            if (startIndex !== undefined) {
                query += "&startIndex=" + startIndex;
            } 
            window.fetch('https://www.googleapis.com/books/v1/volumes?q=' + query + "&maxResults=40&")
                .then(response => response.text())
                .then(text => JSON.parse(text))
                .then(data => {
                        debugger;
                    let table = window.getTableFromResult(data);
                    console.log(table);
                        $('#js-app-book-display').html("");
                        $('#js-app-book-display').append(table);
                    }
                );

            e.preventDefault();
            return false;
        });
    window.getTableFromResult = function (data) {
        let result = `<table class ="table table-responsive table-striped table-hover js-multi-insert">
                    <form>
                        <thead>
                            <tr>
                                <th>Add to bookshelf</th>
                                <th>Image</th>
                                <th>Isbn</th>
                                <th>Title</th>
                                <th>Author</th>
                            </tr>
                        </thead>
                        <tbody>`;
                            console.log(data);
                            for (let item of data.items) {
                            let author = item.volumeInfo.authors,
                            title = item.volumeInfo.title,
                            isbn, image;
                            if (item.volumeInfo.industryIdentifiers !== undefined)
                            if (item.volumeInfo.industryIdentifiers.length > 1)
                                isbn = item.volumeInfo.industryIdentifiers[1].identifier;
                            else
                                isbn = item.volumeInfo.industryIdentifiers[0].identifier;
                            if (item.volumeInfo.imageLinks !== undefined)
                            image = item.volumeInfo.imageLinks.thumbnail;
                            else
                                image = "~/wwwroot/images/no-image.jpg";
                            result += `
                                            <tr>
                                                <input type="hidden" name="isbn" value="${isbn}"/>
                                                <input type="hidden" name="author" value="${author}"/>
                                                <input type="hidden" name="HaveRead" value="${false}"/>
                                                <input type="hidden" name="image" value="${image}" />
                                                <td><input type="checkbox" name="addToBookShelf" />
                                                <td><img src="${image}" /></td>
                                                <td>${isbn}</td>
                                                <td>${title}</td>
                                                <td>${author}</td>
                                            </tr>
                                        `;
                            }
        result += `</tbody>
                    </form>
                    </table>
                    <ul class ="pagination">`;
                   for (let page = 40, index = 0; page < data.totalItems; page += 40, index++) {
                       result += `<li><a href='' data-start_index=${page} class='js-start-index js-search-query'>${index}</a></li>`;
                   }
        result +=  `</ul>
                    <a href="/" class="btn btn-block btn-success js-btn-submit">Submit</a>`;
        return result;
    }
    $('body').on('click', '.js-btn-submit',
        function (e) {
            console.log($('form.js-multi-insert').serialize());
            let form = $('table').find('input').serializeArray();
            var json = new Object();
            json["books"] = new Array();
            let bookArray = json["books"];
            for (let i = 0; i < form.length - 4;) {
                let addToLibrary = form[i + 4];
                if (addToLibrary.name === "addToBookShelf") {
                    let isbn = form[i];
                    let author = form[i + 1];
                    let haveRead = form[i + 2];
                    let image = form[i + 3];
                    let book = new Object();
                    book["image"] = image.value;
                    book["isbn"] = isbn.value;
                    book["author"] = author.value;
                    book["haveRead"] = haveRead.value;
                    book["userId"] = $('#js-user-id').data('user_id');
                    bookArray.push(book);
                    i += 5;
                    continue;
                }
                i += 4;
            }
            console.log('posting the following to the server', JSON.stringify({ books: json.books, userId: $('#js-user-id').data('user_id') }));
            fetch("/book/new",
            {
                method: "POST",
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(json.books)
        }).then(res => console.log(res));
        });
    $('body').on('click',
        '.js-data-modal-load',
        function (e) {
            let query = $(this).data('isbn');
            console.log(this);
            console.log(query);
            window.fetch('https://www.googleapis.com/books/v1/volumes?q=' + query + "&maxResults=1&")
                .then(response => response.text())
                .then(text => JSON.parse(text))
                .then(data => data.items.length > 0 ? data.items[0] : undefined)
                .then(data => {
                    console.log(data);
                    if (data === undefined) {
                        throw new Exception("ISBN doesn't exist or the api is down");
                    }
                    var bookModal = $('#bookModal');
                    bookModal.find('.modal-title').text(data.volumeInfo.title + " Written by: " + data.volumeInfo.authors.join(","));
                    bookModal.find('.modal-body').text(data.volumeInfo.description);
                    bookModal.find('.more-info').attr('href', "/notes/Details/" + query);
                    bookModal.find('.js-delete').attr('href', "/notes/Details/" + query);
                });
        });
})(jQuery);
