let nextHandler = async function (pageIndex) {
    let formData = new FormData();
    formData.append("page", pageIndex);
    let data = await fetch('./Comments', {method: 'POST', body: formData});
    $(".comments").append(await data.text());
    return data.status === 200;

};
let scroller = new InfiniteAjaxScroll('.comments', {
    item: '.comment',
    next: nextHandler,
    spinner: '.spinner',
    delay: 600
})

function commentForm(id, element) {
    let form = document.querySelector(".commentForm");
    document.querySelector("#comment").classList.remove("is-invalid");
    if (element.contains(form)) {
        form.classList.toggle("d-none");
    } else {
        document.querySelector("#comment").value = '';
        document.querySelector("#parentId").setAttribute('value', id);
        element.append(form);
        form.classList.remove("d-none");
    }
}

async function addComment(form) {
    if (!form.checkValidity()) {
        document.querySelector(".invalid-feedback").innerHTML = 'Пожалуйста, введите комментарий.'
        event.preventDefault()
        event.stopPropagation()
        return
    } else if (document.querySelector("#comment").value.length > 500) {
        document.querySelector("#comment").classList.add("is-invalid");
        document.querySelector(".invalid-feedback").innerHTML = 'Длинна коментария не должна превышать 500 символов.'
        return
    }
    scroller.pause();
    let response = await fetch('./AddComment', {method: 'POST', body: new FormData(form)});
    let container = document.querySelector(".commentContainer");
    switch (response.status) {
        case 200:
            form.classList.add("d-none");
            document.querySelector("#comment").value = '';
            document.querySelector("#parentId").setAttribute("value", '');
            container.append(form);
            document.querySelector(".comments").innerHTML = await response.text();
            scroller.pageIndex = 0;
            scroller.resume();
            break;
        case 401:
            document.querySelector("#comment").classList.add("is-invalid");
            document.querySelector(".invalid-feedback").innerHTML = 'Вы должны быть авторизованы.'
            scroller.resume()
    }
}

function scrollToId(id) {
    id = '#com' + id;
    let element = document.querySelector(id);
    element.scrollIntoView({
        behavior: 'smooth',
        block: "center",
        inline: "center"
    });
    element.style.backgroundColor = '#c1ceff'
    setTimeout(() => element.style.backgroundColor = '', 2000)
}

async function deleteComment(id, element) {
    let data = new FormData()
    data.append('id', id)
    let response = await fetch('/Catalog/DeleteComment', {method: 'POST', body: data});
    if (response.status === 200) {
        element.remove();
    } else {
        element.style.backgroundColor = '#ff4d4d'
        setTimeout(() => element.style.backgroundColor = '', 500)
    }
}
