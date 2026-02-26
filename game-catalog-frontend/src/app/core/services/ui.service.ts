import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({ providedIn: 'root' })
export class UiService {

  // This function returns a Promise that resolves to true if the user clicks "Delete"
  confirmDelete(message: string = "You won't be able to revert this!") {
    return Swal.fire({
      title: 'Are you sure?',
      text: message,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    });
  }

  showSuccess(title: string, message: string) {
    Swal.fire(title, message, 'success');
  }

  showError(title: string, message: string) {
    Swal.fire(title, message, 'error');
  }
}