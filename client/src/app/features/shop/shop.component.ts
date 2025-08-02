import { Component, inject } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../core/models/product';

@Component({
  selector: 'app-shop',
  imports: [],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent {
  private shopService = inject(ShopService);
  products: Product[] = [];

  ngOnInit(): void {
    this.shopService.getProducts().subscribe({
      next: response => this.products = response.value.data,
      error: error => console.error(error),
      complete: () => console.log('completed')
    });
  }
}
