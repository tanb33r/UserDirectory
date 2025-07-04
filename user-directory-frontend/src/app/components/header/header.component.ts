import { Component, EventEmitter, Output } from '@angular/core';

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
  selectedSource = 'MSSMS';
  @Output() dataSourceChange = new EventEmitter<string>();

  onSourceChange(event: any) {
    this.selectedSource = event.target.value;
    this.dataSourceChange.emit(this.selectedSource);
  }
}
