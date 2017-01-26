import { Observable } from 'rxjs/Observable';
import { interval } from 'rxjs/observable/interval';
import { Injectable } from '@angular/core';

@Injectable()
export class UploadService {

  private progress$: Observable<number>;
  progress: number = 0;
  progressObserver: any;


  constructor() {
    this.progress$ = new Observable<number>(observer=>{
    this.progressObserver = observer;
    });
  }

  upload(url: string, message: string, files: File[]): Promise<any> {
    console.log(url);
    return new Promise((resolve, reject) => {
      let formData: FormData = new FormData();
      let xhr: XMLHttpRequest = new XMLHttpRequest();

      formData.append("message", message);
      if (files) {
        for (let i = 0; i < files.length; i++) {
          formData.append("uploads[]", files[i], files[i].name);
        }
      }

      xhr.onreadystatechange = () => {
        if (xhr.readyState === 4) {
          if (xhr.status === 200) {
            resolve(JSON.parse(xhr.response));
          }
          else {
            reject(xhr.response);
          }
        }
      };

      UploadService.setUploadUpdateInterval(500);

      if (files) {
        xhr.upload.onprogress = (event) => {
          this.progress = Math.round(event.loaded / event.total * 100);

          if (this.progressObserver) {
            this.progressObserver.next(this.progress);
          }
        };
      }

      xhr.open("PUT", url, true);
      xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
      xhr.setRequestHeader("Content-Disposition", "multipart/form-data");
      xhr.setRequestHeader("enctype", "multipart/form-data");
      xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("jwt"));
      xhr.send(formData);

    });
  }

  static setUploadUpdateInterval(interval: number): void {
    setInterval(() => { }, interval);
  }
}
