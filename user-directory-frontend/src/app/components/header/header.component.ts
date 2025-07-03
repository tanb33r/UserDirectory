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
    { label: 'SQL', value: 'default' },
    { label: 'NoSQL', value: 'alt' }
  ];
  selectedSource = 'default';
  @Output() dataSourceChange = new EventEmitter<string>();

  onSourceChange(event: any) {
    this.selectedSource = event.target.value;
    this.dataSourceChange.emit(this.selectedSource);
  }
}
