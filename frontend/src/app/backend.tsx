import axios, { AxiosInstance } from 'axios';

let backend = axios.create();

function replaceBackend(instance : AxiosInstance) {
    backend = instance;
}

export {
    backend as default,
    replaceBackend
}