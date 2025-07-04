import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { DataSourceService } from '../../services/data-source.service';

import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  dataSources = [
    { label: 'MSSMS', value: 'MSSMS' },
    { label: 'MongoDB', value: 'MongoDB' }
  ];
  selectedSource = 'MSSMS';
  @Output() dataSourceChange = new EventEmitter<string>();

  constructor(private dataSourceService: DataSourceService) {
    this.dataSourceService.getDataSource().subscribe(ds => {
      if (ds) this.selectedSource = ds;
    });
  }

  ngOnInit() {
    // Initialize localStorage with default value if not set
    if (!localStorage.getItem('selectedDataSource')) {
      localStorage.setItem('selectedDataSource', 'MSSMS');
    }
    this.selectedSource = localStorage.getItem('selectedDataSource') || 'MSSMS';
  }

  onSourceChange(event: any) {
    const newSource = event.target.value;
    this.selectedSource = newSource;
    localStorage.setItem('selectedDataSource', newSource);
    this.dataSourceChange.emit(newSource); // Emit the event to notify parent component
  }
}
