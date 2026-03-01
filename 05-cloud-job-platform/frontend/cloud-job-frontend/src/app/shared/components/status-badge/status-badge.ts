import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-status-badge',
  imports: [CommonModule],
  templateUrl: './status-badge.html',
})
export class StatusBadgeComponent {
  @Input() status!: string;

  get color(): string {
    switch (this.status) {
      case 'Completed':
        return 'green';
      case 'Processing':
        return 'orange';
      case 'Failed':
        return 'red';
      default:
        return 'gray';
    }
  }
}
