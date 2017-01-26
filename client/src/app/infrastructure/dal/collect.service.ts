import { ICollectModel } from '../../models/icollect-model';
import { EventEmitter, Injectable } from '@angular/core';
import { UploadService } from './upload.service';

@Injectable()
export class CollectService {

  service: string = "http://localhost:5000/";
  TimelineUpdated: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(
    private uploadService: UploadService
  ) { }

  Collect(collectModel: ICollectModel): Promise<boolean> {
    let model = { text: collectModel.text };
    let files: File[] = collectModel.files;
    return this.uploadService.upload(`${this.service}api/collect`, collectModel.text, files)
      .then(result => this.ForceReloadTimeline(this))
      .catch(this.handleError);
  }

  private handleError(error: any) {
    let errMsg = (error.message) ? error.message :
      error.status ? `$(error.status) - ${error.statusText}` : 'Server error';

    console.error(errMsg);
    throw errMsg;
  }
  private ForceReloadTimeline(that: CollectService) {
    that.TimelineUpdated.next(true);
  }



}
