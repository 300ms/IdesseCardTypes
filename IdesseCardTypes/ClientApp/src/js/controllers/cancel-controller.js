import {Controller} from "@hotwired/stimulus";

export default class extends Controller {
    connect() {
        this.sayHi('Test');
    }
    
    sayHi(controllerName) {
        console.log(`Hello from the ${controllerName} controller!`, this.element);
    }
}