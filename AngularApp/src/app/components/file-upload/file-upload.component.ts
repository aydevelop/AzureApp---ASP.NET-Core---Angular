import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { FileUploadService } from 'src/app/services/file-upload.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent {
  email = ''; 
  selectedFiles?: FileList;
  currentFile?: File;
  progress = 0;
  message = '';
  disabledButton = true;
  done = false;

  fileInfos?: Observable<any>;
  
  constructor(private uploadService: FileUploadService) { }

  selectFile(event: any): void {
    this.selectedFiles = event.target.files;
    this.disabledButton = false;
  }

  selectEmail(event: any): void {
    this.email = event.target.value;
  }
  
  upload(): void {
    this.progress = 0;

    if (this.selectedFiles) {
      const file: File | null = this.selectedFiles.item(0);

      if (file) {
        this.currentFile = file;

        this.uploadService.upload(this.email, this.currentFile).subscribe({
          next: (event: any) => {
            if (event.type === HttpEventType.UploadProgress) {
              this.progress = Math.round(100 * event.loaded / event.total);
            } else if (event instanceof HttpResponse) {
              this.done = true;
            }
          },
          error: (err: any) => {
            console.log(err);
            this.progress = 0;

            if (err?.error?.errors) {
              var obj = err?.error?.errors;
              this.message = Object.keys(obj).map(function(k) { return k + ": " + obj[k] }).join(", ")
            } else {
              this.message = 'Could not upload the file!';
            }

            this.currentFile = undefined;
            this.disabledButton = true;
          }
        });
      }

      this.selectedFiles = undefined;
    }
  }
}
