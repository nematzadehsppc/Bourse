import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.css'],
})
export class UploadFileComponent implements OnInit {
  public progress: number;
  public: string = '';
  @Output() public onUploadFinished = new EventEmitter();

  //constructor() { }
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    //debugger;
    this.http.post('http://localhost:10818/paramvalue/upload', formData, {
      reportProgress: true,
      observe: 'events'
    })
      .subscribe(events => {
        if (events.type == HttpEventType.UploadProgress ) {
          alert('Upload progress: ' + Math.round(events.loaded / events.total * 100) + '%');
        } else if (events.type === HttpEventType.Response) {
          alert(events);
        }
        //alert(events);
        alert(events.type);
      }
        //,
        //e => {
        //  alert(e);
        //}
      );
  }

}
