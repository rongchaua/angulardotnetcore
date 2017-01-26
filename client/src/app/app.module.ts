import { UploadService } from './infrastructure/dal/upload.service';
import { CollectService } from './infrastructure/dal/collect.service';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { CollectComponent } from './components/collect/collect.component';
import { SearchComponent } from './components/search/search.component';
import { KnowledgeCollectorComponent } from './components/knowledge-collector/knowledge-collector.component';
import { KnowledgeTimelineComponent } from './components/knowledge-timeline/knowledge-timeline.component';

@NgModule({
  declarations: [
    AppComponent,
    CollectComponent,
    SearchComponent,
    KnowledgeCollectorComponent,
    KnowledgeTimelineComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [
    CollectService,
    UploadService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
