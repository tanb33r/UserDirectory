import { Component, EventEmitter, Output } from '@angular/core';
import { DataSourceService } from '../../services/data-source.service';

import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  dataSources = [
    { label: 'MSSMS', value: 'MSSMS' },
    { label: 'MongoDB', value: 'MongoDB' }
  ];
  
  selectedSource = '';
  @Output() dataSourceChange = new EventEmitter<string>();

  constructor(private dataSourceService: DataSourceService) {
    this.dataSourceService.getDataSource().subscribe(ds => {
      if (ds) this.selectedSource = ds;
    });
  }

  onSourceChange(event: any) {
    const newSource = event.target.value;
    this.selectedSource = newSource;
    localStorage.setItem('selectedDataSource', newSource);
    this.dataSourceChange.emit(newSource);
  }
}
