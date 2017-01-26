import { ICollectModel } from '../../models/icollect-model';
import { CollectService } from '../../infrastructure/dal/collect.service';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-knowledge-collector',
  templateUrl: './knowledge-collector.component.html',
  styleUrls: ['./knowledge-collector.component.css']
})
export class KnowledgeCollectorComponent implements OnInit {
  text:string = "";
  files:File[];
  fileSelector: any;

  constructor(
    private collectService: CollectService
  ) { }

  ngOnInit() {
  }

  save(){
    let model: ICollectModel = {
      text:this.text,
      files: this.files
    }

    this.collectService.Collect(model)
      .then(success =>{
        this.text = '';
        this.files = [];
        if (this.fileSelector){
          this.fileSelector.form.reset();
        }
      });
    
  }

  onChange($event){
    console.log($event);
    this.fileSelector = $event.target || $event.srcElement;
    this.files =this.fileSelector.files;    
  }

}
