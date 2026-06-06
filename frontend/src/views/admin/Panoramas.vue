<template>
  <div class="space-y-6">
    <!-- Header with Back navigation -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-white p-6 rounded-2xl border border-slate-200/60 shadow-sm">
      <div class="space-y-1">
        <div class="flex items-center space-x-2 text-xs text-slate-500 font-medium">
          <router-link to="/admin/virtual-tours" class="hover:text-teal-600 transition flex items-center">
            <ChevronLeftIcon class="w-3.5 h-3.5 mr-0.5" />
            <span>Quản lý Tour ảo</span>
          </router-link>
          <span>/</span>
          <span class="text-slate-400">Quản lý Cảnh</span>
        </div>
        <h2 class="text-lg font-bold text-slate-800">
          Cảnh Panorama: <span class="text-teal-600 font-extrabold">{{ tour?.title || 'Đang tải...' }}</span>
        </h2>
        <p class="text-xs text-slate-500">
          Thiết lập danh sách cảnh 360°, sắp xếp thứ tự hiển thị và định dạng góc nhìn mặc định cho khách du lịch.
        </p>
      </div>

      <!-- Action Buttons -->
      <div class="flex items-center space-x-2 w-full sm:w-auto">
        <!-- Bulk upload button -->
        <button 
          @click="triggerBulkUpload"
          class="flex-grow sm:flex-grow-0 inline-flex items-center justify-center space-x-1.5 px-4 py-2.5 bg-slate-900 hover:bg-slate-800 text-white font-semibold rounded-xl text-sm transition"
          :disabled="bulkUploading"
        >
          <Loader2Icon v-if="bulkUploading" class="w-4 h-4 animate-spin" />
          <UploadIcon v-else class="w-4 h-4" />
          <span>{{ bulkUploading ? 'Đang tải lên...' : 'Tải lên hàng loạt' }}</span>
        </button>
        <input 
          ref="bulkFilesInput"
          type="file"
          multiple
          accept="image/*"
          class="hidden"
          @change="handleBulkUpload"
        />

        <!-- Add single panorama button -->
        <button 
          @click="openCreateModal"
          class="flex-grow sm:flex-grow-0 inline-flex items-center justify-center space-x-1.5 px-4 py-2.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-teal-500/10 active:scale-[0.98] transition"
        >
          <PlusIcon class="w-4 h-4" />
          <span>Thêm cảnh</span>
        </button>
      </div>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="bg-white rounded-2xl border border-slate-200/60 shadow-sm p-6 space-y-4">
      <div v-for="i in 3" :key="i" class="flex items-center justify-between py-4 border-b border-slate-100 last:border-0 animate-pulse">
        <div class="flex items-center space-x-4">
          <div class="w-20 h-14 bg-slate-100 rounded-lg"></div>
          <div class="space-y-2">
            <div class="h-4 w-40 bg-slate-100 rounded"></div>
            <div class="h-3 w-20 bg-slate-100 rounded"></div>
          </div>
        </div>
        <div class="h-8 w-24 bg-slate-100 rounded-lg"></div>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-else-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl flex items-center space-x-3">
      <AlertCircleIcon class="w-5 h-5 flex-shrink-0" />
      <span class="text-sm font-medium">{{ error }}</span>
    </div>

    <!-- Empty State -->
    <div v-else-if="panoramas.length === 0" class="bg-white py-16 text-center rounded-2xl border border-slate-200/60 shadow-sm space-y-4">
      <div class="inline-flex p-4 bg-slate-50 text-slate-400 rounded-full border border-slate-100">
        <CompassIcon class="w-10 h-10" />
      </div>
      <div class="space-y-1">
        <h3 class="text-base font-bold text-slate-700">Chưa có cảnh 360° nào</h3>
        <p class="text-xs text-slate-500 max-w-sm mx-auto">Vui lòng tải lên hoặc chọn ảnh panorama từ thư viện để thiết lập tour ảo.</p>
      </div>
    </div>

    <!-- Grid Layout of Panoramas -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div 
        v-for="(pano, index) in panoramas" 
        :key="pano.id"
        class="bg-white rounded-2xl border border-slate-200/60 shadow-sm overflow-hidden flex flex-col group hover:shadow-md hover:border-slate-300/60 transition duration-200"
      >
        <!-- Photo Container -->
        <div class="aspect-[16/10] w-full bg-slate-100 border-b border-slate-100 relative overflow-hidden flex items-center justify-center text-slate-400">
          <img 
            :src="pano.panoramaImageUrl" 
            class="w-full h-full object-cover group-hover:scale-[1.03] transition duration-300"
            alt="Panorama scene"
          />
          
          <!-- Badges -->
          <div class="absolute top-4 left-4 flex flex-col gap-2">
            <span 
              v-if="tour?.defaultPanoramaId === pano.id"
              class="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-bold bg-teal-500 text-slate-950 shadow-sm"
            >
              <CheckIcon class="w-3.5 h-3.5 mr-1" />
              Mặc định
            </span>
            <span class="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-semibold bg-slate-900/70 text-white shadow-sm font-mono">
              Thứ tự: {{ pano.displayOrder }}
            </span>
            <span 
              :class="[
                'inline-flex items-center px-2.5 py-1 rounded-full text-[10px] font-bold shadow-sm',
                pano.status === 'published' ? 'bg-emerald-500 text-slate-950' : 'bg-amber-500 text-slate-950'
              ]"
            >
              {{ pano.status === 'published' ? 'Đã xuất bản' : 'Bản nháp' }}
            </span>
          </div>

          <!-- Quick setting default indicator -->
          <button
            v-if="tour?.defaultPanoramaId !== pano.id"
            @click="setDefaultPanorama(pano.id)"
            class="absolute top-4 right-4 p-2 bg-white/95 text-slate-700 hover:text-teal-600 rounded-full shadow-md hover:scale-105 transition"
            title="Đặt làm cảnh mặc định của tour"
          >
            <StarIcon class="w-4 h-4" />
          </button>
        </div>

        <!-- Details -->
        <div class="p-5 flex-grow flex flex-col justify-between space-y-4">
          <div class="space-y-1.5">
            <h3 class="font-bold text-slate-800 text-sm line-clamp-1">{{ pano.title }}</h3>
            <p class="text-xs text-slate-400 line-clamp-2">{{ pano.description || 'Không có mô tả cảnh.' }}</p>
            
            <div class="text-[10px] text-slate-500 font-mono flex gap-3">
              <span>Yaw: {{ parseFloat(pano.initialYaw).toFixed(2) }}</span>
              <span>Pitch: {{ parseFloat(pano.initialPitch).toFixed(2) }}</span>
              <span>Fov: {{ parseFloat(pano.initialFov).toFixed(0) }}°</span>
            </div>
          </div>

          <div class="pt-4 border-t border-slate-100 flex items-center justify-between">
            <!-- Order navigation controls -->
            <div class="flex items-center space-x-1">
              <button
                @click="moveOrder(index, -1)"
                :disabled="index === 0"
                class="p-1.5 bg-slate-50 hover:bg-slate-100 rounded border border-slate-200 text-slate-500 disabled:opacity-40 disabled:cursor-not-allowed"
                title="Di chuyển lên"
              >
                <ArrowUpIcon class="w-3.5 h-3.5" />
              </button>
              <button
                @click="moveOrder(index, 1)"
                :disabled="index === panoramas.length - 1"
                class="p-1.5 bg-slate-50 hover:bg-slate-100 rounded border border-slate-200 text-slate-500 disabled:opacity-40 disabled:cursor-not-allowed"
                title="Di chuyển xuống"
              >
                <ArrowDownIcon class="w-3.5 h-3.5" />
              </button>
            </div>

            <!-- Management action buttons -->
            <div class="flex items-center space-x-1">
              <!-- Initial viewport viewer button -->
              <button
                @click="openInitialViewModal(pano)"
                class="p-2 hover:bg-teal-55 rounded-lg text-slate-500 hover:text-teal-600 transition"
                title="Đặt góc nhìn ban đầu"
              >
                <CompassIcon class="w-4 h-4" />
              </button>

              <!-- Hotspot editor button -->
              <router-link
                :to="'/admin/panoramas/' + pano.id + '/hotspots'"
                class="p-2 hover:bg-slate-100 rounded-lg text-slate-500 hover:text-teal-600 transition flex items-center justify-center"
                title="Thiết lập điểm tương tác (Hotspot)"
              >
                <PinIcon class="w-4 h-4" />
              </router-link>

              <!-- Edit button -->
              <button
                @click="openEditModal(pano)"
                class="p-2 hover:bg-slate-100 rounded-lg text-slate-500 hover:text-indigo-600 transition"
                title="Chỉnh sửa cảnh"
              >
                <EditIcon class="w-4 h-4" />
              </button>

              <!-- Delete button -->
              <button
                @click="confirmDelete(pano)"
                class="p-2 hover:bg-slate-100 rounded-lg text-slate-500 hover:text-red-600 transition"
                title="Xóa cảnh"
              >
                <TrashIcon class="w-4 h-4" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Form (Create / Edit) -->
    <div 
      v-if="showFormModal" 
      class="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeFormModal"
    >
      <div class="bg-white rounded-2xl w-full max-w-lg shadow-2xl border border-slate-100 overflow-hidden flex flex-col">
        <!-- Modal Header -->
        <div class="p-6 border-b border-slate-100 flex justify-between items-center">
          <div class="space-y-1">
            <h3 class="text-base font-bold text-slate-800">
              {{ isEditMode ? 'Chỉnh sửa cảnh Panorama' : 'Thêm cảnh Panorama mới' }}
            </h3>
            <p class="text-xs text-slate-500 font-medium">
              Nhập các thông tin chi tiết và tải ảnh panorama 360 độ.
            </p>
          </div>
          <button 
            type="button"
            @click="closeFormModal"
            class="p-1.5 hover:bg-slate-100 rounded-lg text-slate-400 hover:text-slate-600 transition"
          >
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- Modal Body -->
        <form @submit.prevent="submitForm" class="p-6 space-y-4">
          <!-- Title -->
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Tiêu đề cảnh *</label>
            <input 
              v-model="modalForm.title"
              type="text"
              required
              placeholder="Ví dụ: Cổng chính, Bên trong chính điện..."
              class="w-full px-4 py-2.5 bg-slate-50/80 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white transition"
            />
          </div>

          <!-- Description -->
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Mô tả ngắn</label>
            <textarea 
              v-model="modalForm.description"
              rows="2"
              placeholder="Mô tả sơ lược về góc cảnh hoặc vị trí đứng này..."
              class="w-full px-4 py-2.5 bg-slate-50/80 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white transition resize-none"
            ></textarea>
          </div>

          <!-- Display Order -->
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Thứ tự hiển thị *</label>
            <input 
              v-model.number="modalForm.displayOrder"
              type="number"
              required
              class="w-full px-4 py-2.5 bg-slate-50/80 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white transition font-mono"
            />
          </div>

          <!-- Status -->
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Trạng thái *</label>
            <select 
              v-model="modalForm.status"
              required
              class="w-full px-4 py-2.5 bg-slate-50/80 border border-slate-200 rounded-xl text-sm focus:outline-none focus:border-teal-500/80 focus:bg-white transition"
            >
              <option value="draft">Bản nháp (Draft)</option>
              <option value="published">Xuất bản (Published)</option>
              <option value="archived">Lưu trữ (Archived)</option>
            </select>
          </div>

          <!-- Panorama Image Picker -->
          <div>
            <label class="block text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2">Hình ảnh Panorama 360° *</label>
            <div class="relative w-full aspect-[2/1] rounded-xl overflow-hidden border border-slate-200 bg-slate-50 flex flex-col items-center justify-center group">
              <img 
                v-if="modalForm.panoramaImageUrl" 
                :src="modalForm.panoramaImageUrl" 
                class="w-full h-full object-cover" 
              />
              <div v-else class="text-center p-6 space-y-2 text-slate-400">
                <CompassIcon class="w-8 h-8 mx-auto animate-pulse text-slate-300" />
                <p class="text-xs">Chưa chọn hình ảnh toàn cảnh</p>
              </div>
              
              <div 
                v-if="modalForm.panoramaImageUrl" 
                class="absolute inset-0 bg-slate-900/60 opacity-0 group-hover:opacity-100 transition flex items-center justify-center space-x-2"
              >
                <button 
                  type="button"
                  @click="openMediaSelector"
                  class="px-3 py-1.5 bg-white text-slate-900 rounded-lg text-xs font-bold hover:bg-slate-50 transition"
                >
                  Thay đổi
                </button>
              </div>
            </div>

            <div v-if="!modalForm.panoramaImageUrl" class="mt-2 flex items-center space-x-2">
              <button
                type="button"
                @click="openMediaSelector"
                class="py-2 px-3 border border-slate-200 hover:bg-slate-50 text-slate-700 font-semibold rounded-xl text-xs transition flex items-center space-x-1"
              >
                <ImageIcon class="w-3.5 h-3.5" />
                <span>Chọn từ thư viện</span>
              </button>
              
              <button
                type="button"
                @click="triggerDirectUpload"
                class="py-2 px-3 bg-slate-900 hover:bg-slate-800 text-white font-semibold rounded-xl text-xs transition flex items-center space-x-1"
              >
                <UploadIcon class="w-3.5 h-3.5" />
                <span>Tải ảnh mới</span>
              </button>
              <input 
                ref="directUploadInput" 
                type="file" 
                accept="image/*" 
                class="hidden" 
                @change="handleDirectUpload"
              />
            </div>
          </div>

          <!-- Error Message inside Modal -->
          <div v-if="modalError" class="text-xs font-semibold text-red-600 bg-red-50 p-2.5 rounded-lg border border-red-100">
            {{ modalError }}
          </div>

          <!-- Actions -->
          <div class="flex items-center space-x-3 pt-4 border-t border-slate-100">
            <button 
              type="submit" 
              class="flex-grow py-2.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-teal-500/10 active:scale-[0.98] transition flex items-center justify-center space-x-2 disabled:opacity-50"
              :disabled="submitting"
            >
              <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
              <span>{{ isEditMode ? 'Lưu cập nhật' : 'Tạo cảnh' }}</span>
            </button>
            <button 
              type="button"
              @click="closeFormModal"
              class="px-4 py-2.5 border border-slate-200 hover:bg-slate-50 text-slate-600 font-semibold rounded-xl text-sm transition"
              :disabled="submitting"
            >
              Hủy bỏ
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Photo Sphere Viewer Initial View Modal -->
    <div 
      v-if="showInitialViewModal" 
      class="fixed inset-0 z-50 overflow-y-auto bg-slate-900/80 backdrop-blur-sm flex items-center justify-center p-4"
    >
      <div class="bg-slate-950 text-slate-100 rounded-2xl w-full max-w-4xl shadow-2xl border border-slate-800/80 overflow-hidden flex flex-col">
        <!-- Header -->
        <div class="p-6 border-b border-slate-800 flex justify-between items-center">
          <div class="space-y-1">
            <h3 class="text-base font-bold text-white flex items-center space-x-2">
              <CompassIcon class="w-5 h-5 text-teal-400" />
              <span>Đặt góc nhìn ban đầu</span>
            </h3>
            <p class="text-xs text-slate-400">
              Xoay chuột hoặc phóng to/thu nhỏ để chọn góc nhìn lý tưởng nhất cho du khách khi mới mở cảnh này.
            </p>
          </div>
          <button 
            type="button"
            @click="closeInitialViewModal"
            class="p-1.5 hover:bg-slate-900 rounded-lg text-slate-400 hover:text-slate-200 transition"
          >
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- 360 Viewer Canvas Container -->
        <div class="relative w-full h-[450px] bg-black">
          <div ref="viewerContainer" class="w-full h-full"></div>
          
          <!-- Crosshair Center Overlay -->
          <div class="absolute inset-0 pointer-events-none flex items-center justify-center opacity-30">
            <div class="w-6 h-6 border-2 border-dashed border-teal-400 rounded-full flex items-center justify-center">
              <div class="w-1.5 h-1.5 bg-teal-400 rounded-full"></div>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="p-6 border-t border-slate-800 bg-slate-900/50 flex items-center justify-between">
          <div class="text-xs text-slate-400 font-mono hidden sm:block">
            Nhấn và kéo để điều chỉnh camera
          </div>
          <div class="flex items-center space-x-3 w-full sm:w-auto">
            <button 
              type="button"
              @click="saveInitialView"
              class="flex-grow sm:flex-grow-0 px-6 py-2.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-xl shadow-lg hover:shadow-teal-500/10 active:scale-[0.98] transition flex items-center justify-center space-x-2 disabled:opacity-50"
              :disabled="savingInitialView"
            >
              <Loader2Icon v-if="savingInitialView" class="w-4 h-4 animate-spin" />
              <span>Lưu góc nhìn hiện tại</span>
            </button>
            <button 
              type="button"
              @click="closeInitialViewModal"
              class="px-5 py-2.5 bg-slate-900 hover:bg-slate-850 border border-slate-800 text-slate-300 font-semibold rounded-xl text-sm transition"
              :disabled="savingInitialView"
            >
              Hủy bỏ
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Media Library Selector Modal -->
    <div 
      v-if="showMediaSelector" 
      class="fixed inset-0 z-[60] overflow-y-auto bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeMediaSelector"
    >
      <div class="bg-white rounded-2xl w-full max-w-2xl shadow-2xl border border-slate-100 overflow-hidden flex flex-col max-h-[80vh]">
        <!-- Header -->
        <div class="p-6 border-b border-slate-100 flex justify-between items-center">
          <div class="space-y-1">
            <h3 class="text-base font-bold text-slate-800">Chọn ảnh từ thư viện</h3>
            <p class="text-xs text-slate-500 font-medium">Chỉ hỗ trợ ảnh panorama hoặc ảnh thường.</p>
          </div>
          <button 
            type="button"
            @click="closeMediaSelector"
            class="p-1.5 hover:bg-slate-100 rounded-lg text-slate-400 hover:text-slate-600 transition"
          >
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- Modal Upload Section inside Selector -->
        <div class="px-6 py-4 bg-slate-50 border-b border-slate-100 flex items-center justify-between">
          <span class="text-xs text-slate-500 font-medium">Tải ảnh mới:</span>
          <button 
            type="button"
            @click="triggerModalUpload"
            class="px-3 py-1.5 bg-teal-500 hover:bg-teal-600 text-slate-950 font-bold rounded-lg text-xs transition flex items-center space-x-1"
            :disabled="modalUploading"
          >
            <Loader2Icon v-if="modalUploading" class="w-3 h-3 animate-spin" />
            <UploadIcon v-else class="w-3 h-3" />
            <span>Tải lên</span>
          </button>
          <input 
            ref="modalUploadInput" 
            type="file" 
            accept="image/*" 
            class="hidden" 
            @change="handleModalUpload"
          />
        </div>

        <!-- Content Area -->
        <div class="p-6 overflow-y-auto flex-1 min-h-[200px]">
          <div v-if="selectorLoading" class="grid grid-cols-4 gap-3 animate-pulse">
            <div v-for="i in 8" :key="i" class="aspect-square bg-slate-100 rounded-xl"></div>
          </div>

          <div v-else-if="selectorImages.length === 0" class="text-center py-12 space-y-3">
            <ImageIcon class="w-8 h-8 mx-auto text-slate-300" />
            <p class="text-sm font-medium text-slate-500">Thư viện ảnh trống.</p>
          </div>

          <div v-else class="grid grid-cols-4 gap-3">
            <div 
              v-for="img in selectorImages" 
              :key="img.id"
              :class="[
                'aspect-square rounded-xl overflow-hidden cursor-pointer border-2 transition relative group bg-slate-50',
                modalForm.panoramaImageId === img.id ? 'border-teal-500 shadow-md ring-2 ring-teal-500/20' : 'border-slate-100 hover:border-slate-300'
              ]"
              @click="selectImage(img)"
            >
              <img :src="img.fileUrl" class="w-full h-full object-cover" />
              <div 
                v-if="modalForm.panoramaImageId === img.id"
                class="absolute inset-0 bg-teal-500/10 flex items-center justify-center"
              >
                <div class="p-1 bg-teal-500 text-slate-950 rounded-full shadow-md">
                  <CheckIcon class="w-3 h-3" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Footer Pagination -->
        <div 
          v-if="selectorImages.length > 0 && selectorTotalPages > 1"
          class="p-4 bg-slate-50 border-t border-slate-100 flex justify-between items-center text-xs"
        >
          <span class="text-slate-500 font-medium">Trang {{ selectorPage }} / {{ selectorTotalPages }}</span>
          <div class="flex items-center space-x-2">
            <button 
              type="button"
              @click="selectorPrevPage"
              :disabled="selectorPage === 1"
              class="px-2.5 py-1 bg-white border border-slate-200 rounded-md hover:bg-slate-50 transition disabled:opacity-40"
            >
              Trước
            </button>
            <button 
              type="button"
              @click="selectorNextPage"
              :disabled="selectorPage === selectorTotalPages"
              class="px-2.5 py-1 bg-white border border-slate-200 rounded-md hover:bg-slate-50 transition disabled:opacity-40"
            >
              Sau
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick } from 'vue'
import { useRoute } from 'vue-router'
import api from '../../utils/api'
import { useToastStore } from '../../stores/toast'
import { useConfirmStore } from '../../stores/confirm'
import { Viewer } from '@photo-sphere-viewer/core'
import '@photo-sphere-viewer/core/index.css'
import { 
  Plus as PlusIcon, 
  Image as ImageIcon, 
  Edit as EditIcon, 
  Trash as TrashIcon, 
  AlertCircle as AlertCircleIcon,
  Compass as CompassIcon,
  X as XIcon,
  Loader2 as Loader2Icon,
  Upload as UploadIcon,
  Check as CheckIcon,
  ChevronLeft as ChevronLeftIcon,
  ArrowUp as ArrowUpIcon,
  ArrowDown as ArrowDownIcon,
  Star as StarIcon,
  Pin as PinIcon
} from 'lucide-vue-next'

const toastStore = useToastStore()
const confirmStore = useConfirmStore()

const route = useRoute()
const tourId = ref(route.params.id)

const loading = ref(false)
const error = ref(null)

const tour = ref(null)
const panoramas = ref([])

// Modal form state
const showFormModal = ref(false)
const isEditMode = ref(false)
const submitting = ref(false)
const modalError = ref(null)
const editingPanoId = ref(null)

const modalForm = ref({
  title: '',
  description: '',
  displayOrder: 0,
  panoramaImageId: null,
  panoramaImageUrl: null,
  status: 'published',
  initialYaw: 0,
  initialPitch: 0,
  initialFov: 90
})

// Image Selector state
const showMediaSelector = ref(false)
const selectorLoading = ref(false)
const selectorImages = ref([])
const selectorPage = ref(1)
const selectorTotalPages = ref(1)
const selectorPageSize = ref(12)

const directUploadInput = ref(null)
const modalUploadInput = ref(null)
const modalUploading = ref(false)

// Bulk Upload files
const bulkFilesInput = ref(null)
const bulkUploading = ref(false)

// Photo Sphere Viewer modal state
const showInitialViewModal = ref(false)
const viewerContainer = ref(null)
const activePano = ref(null)
const savingInitialView = ref(false)
let viewerInstance = null

// FOV to Zoom conversion helpers
const minFov = 30
const maxFov = 90
const fovToZoomLevel = (fov) => {
  const clampedFov = Math.max(minFov, Math.min(maxFov, fov))
  return ((maxFov - clampedFov) / (maxFov - minFov)) * 100
}
const zoomLevelToFov = (zoomLevel) => {
  return maxFov - (zoomLevel / 100) * (maxFov - minFov)
}

const fetchPanoramas = async () => {
  loading.value = true
  error.value = null
  try {
    const [tourRes, panRes] = await Promise.all([
      api.get(`/api/admin/virtual-tours/${tourId.value}`),
      api.get(`/api/admin/virtual-tours/${tourId.value}/panoramas`)
    ])

    if (tourRes.success) {
      tour.value = tourRes.data
    }
    if (panRes.success) {
      panoramas.value = panRes.data
    }
  } catch (err) {
    error.value = err.message || 'Lỗi khi tải danh sách cảnh.'
  } finally {
    loading.value = false
  }
}

// Set Default Panorama for the Tour
const setDefaultPanorama = async (panoramaId) => {
  try {
    const response = await api.patch(`/api/admin/virtual-tours/${tourId.value}/default-panorama`, {
      defaultPanoramaId: panoramaId
    })
    if (response.success) {
      tour.value.defaultPanoramaId = panoramaId
      toastStore.success('Đặt cảnh mặc định cho tour ảo thành công.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể đặt cảnh mặc định.')
  }
}

// Move item display order locally and submit to server
const moveOrder = async (index, direction) => {
  const targetIndex = index + direction
  if (targetIndex < 0 || targetIndex >= panoramas.value.length) return

  // Swap elements in the array
  const temp = panoramas.value[index]
  panoramas.value[index] = panoramas.value[targetIndex]
  panoramas.value[targetIndex] = temp

  // Fix displayOrder values locally
  panoramas.value.forEach((p, idx) => {
    p.displayOrder = idx
  })

  // Submit bulk reorder request
  try {
    const orderedIds = panoramas.value.map(p => p.id)
    await api.patch(`/api/admin/virtual-tours/${tourId.value}/panoramas/reorder`, {
      orderedIds: orderedIds
    })
  } catch (err) {
    toastStore.error(err.message || 'Lỗi xảy ra khi thay đổi thứ tự.')
    await fetchPanoramas() // Revert to database state if fails
  }
}

const openCreateModal = () => {
  isEditMode.value = false
  modalError.value = null
  modalForm.value = {
    title: '',
    description: '',
    displayOrder: panoramas.value.length,
    panoramaImageId: null,
    panoramaImageUrl: null,
    status: 'published',
    initialYaw: 0,
    initialPitch: 0,
    initialFov: 90
  }
  showFormModal.value = true
}

const openEditModal = (pano) => {
  isEditMode.value = true
  modalError.value = null
  editingPanoId.value = pano.id
  modalForm.value = {
    title: pano.title,
    description: pano.description || '',
    displayOrder: pano.displayOrder,
    panoramaImageId: pano.panoramaImageId,
    panoramaImageUrl: pano.panoramaImageUrl,
    status: pano.status || 'draft',
    initialYaw: pano.initialYaw || 0,
    initialPitch: pano.initialPitch || 0,
    initialFov: pano.initialFov || 90
  }
  showFormModal.value = true
}

const closeFormModal = () => {
  showFormModal.value = false
}

const submitForm = async () => {
  if (!modalForm.value.title.trim()) {
    modalError.value = 'Vui lòng nhập tiêu đề cảnh.'
    return
  }
  if (!modalForm.value.panoramaImageId) {
    modalError.value = 'Vui lòng chọn hoặc tải ảnh panorama 360°.'
    return
  }

  submitting.value = true
  modalError.value = null
  try {
    let response
    if (isEditMode.value) {
      response = await api.put(`/api/admin/panoramas/${editingPanoId.value}`, {
        title: modalForm.value.title,
        description: modalForm.value.description,
        panoramaImageId: modalForm.value.panoramaImageId,
        displayOrder: modalForm.value.displayOrder,
        thumbnailId: null,
        status: modalForm.value.status,
        initialYaw: parseFloat(modalForm.value.initialYaw || 0),
        initialPitch: parseFloat(modalForm.value.initialPitch || 0),
        initialFov: parseFloat(modalForm.value.initialFov || 90)
      })
    } else {
      response = await api.post(`/api/admin/virtual-tours/${tourId.value}/panoramas`, {
        title: modalForm.value.title,
        description: modalForm.value.description,
        panoramaImageId: modalForm.value.panoramaImageId,
        displayOrder: modalForm.value.displayOrder,
        thumbnailId: null,
        initialYaw: parseFloat(modalForm.value.initialYaw || 0),
        initialPitch: parseFloat(modalForm.value.initialPitch || 0),
        initialFov: parseFloat(modalForm.value.initialFov || 90)
      })
    }

    if (response.success) {
      toastStore.success(isEditMode.value ? 'Cập nhật cảnh thành công!' : 'Thêm cảnh mới thành công!')
      showFormModal.value = false
      await fetchPanoramas()
    }
  } catch (err) {
    modalError.value = err.message || 'Lỗi xảy ra khi lưu thông tin cảnh.'
  } finally {
    submitting.value = false
  }
}

const confirmDelete = async (pano) => {
  const confirmResult = await confirmStore.show({
    title: 'Xóa cảnh toàn cảnh',
    message: `Bạn có chắc muốn xóa cảnh "${pano.title}"? Thao tác này không thể hoàn tác.`
  })
  if (!confirmResult) return

  try {
    const response = await api.delete(`/api/admin/panoramas/${pano.id}`)
    if (response.success) {
      toastStore.success('Xóa cảnh thành công.')
      await fetchPanoramas()
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể xóa cảnh.')
  }
}

// Media Selector methods
const openMediaSelector = async () => {
  selectorPage.value = 1
  showMediaSelector.value = true
  await fetchSelectorImages()
}

const closeMediaSelector = () => {
  showMediaSelector.value = false
}

const fetchSelectorImages = async () => {
  selectorLoading.value = true
  try {
    // Fetch media with type panorama or image
    const response = await api.get('/api/admin/media', {
      params: {
        page: selectorPage.value,
        pageSize: selectorPageSize.value
      }
    })
    if (response.success) {
      // Filter image and panorama types
      selectorImages.value = response.data.items.filter(m => m.mediaType === 'panorama' || m.mediaType === 'image')
      selectorPage.value = response.data.page
      selectorTotalPages.value = response.data.totalPages
    }
  } catch (err) {
    console.error('Không thể tải thư viện ảnh.', err)
  } finally {
    selectorLoading.value = false
  }
}

const selectImage = (img) => {
  modalForm.value.panoramaImageId = img.id
  modalForm.value.panoramaImageUrl = img.fileUrl
  closeMediaSelector()
}

const triggerDirectUpload = () => {
  directUploadInput.value.click()
}

const triggerModalUpload = () => {
  modalUploadInput.value.click()
}

const handleDirectUpload = async (e) => {
  const file = e.target.files?.[0]
  if (!file) return
  
  const formData = new FormData()
  formData.append('file', file)
  formData.append('mediaType', 'panorama')

  submitting.value = true
  try {
    const response = await api.post('/api/admin/media/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
    if (response.success) {
      modalForm.value.panoramaImageId = response.data.id
      modalForm.value.panoramaImageUrl = response.data.fileUrl
      toastStore.success('Tải ảnh panorama lên thành công.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi tải ảnh lên.')
  } finally {
    submitting.value = false
    e.target.value = ''
  }
}

const handleModalUpload = async (e) => {
  const file = e.target.files?.[0]
  if (!file) return
  
  const formData = new FormData()
  formData.append('file', file)
  formData.append('mediaType', 'panorama')

  modalUploading.value = true
  try {
    const response = await api.post('/api/admin/media/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
    if (response.success) {
      selectorPage.value = 1
      await fetchSelectorImages()
      toastStore.success('Tải ảnh panorama lên thành công.')
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi tải ảnh lên.')
  } finally {
    modalUploading.value = false
    e.target.value = ''
  }
}

// Bulk uploading process
const triggerBulkUpload = () => {
  bulkFilesInput.value.click()
}

const handleBulkUpload = async (e) => {
  const files = e.target.files
  if (!files || files.length === 0) return

  bulkUploading.value = true
  try {
    const uploadRequests = []
    for (let i = 0; i < files.length; i++) {
      const file = files[i]
      const formData = new FormData()
      formData.append('file', file)
      formData.append('mediaType', 'panorama')
      
      const response = await api.post('/api/admin/media/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
      
      if (response.success) {
        const title = file.name.substring(0, file.name.lastIndexOf('.')) || file.name
        uploadRequests.push({
          title: title,
          description: '',
          panoramaImageId: response.data.id,
          thumbnailId: null,
          displayOrder: panoramas.value.length + i,
          initialYaw: 0,
          initialPitch: 0,
          initialFov: 90
        })
      }
    }

    if (uploadRequests.length > 0) {
      const bulkResponse = await api.post(`/api/admin/virtual-tours/${tourId.value}/panoramas/bulk`, uploadRequests)
      if (bulkResponse.success) {
        toastStore.success(`Đã tải lên hàng loạt ${uploadRequests.length} ảnh thành công.`)
        await fetchPanoramas()
      }
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi tải lên hàng loạt.')
  } finally {
    bulkUploading.value = false
    e.target.value = ''
  }
}

const selectorPrevPage = async () => {
  if (selectorPage.value > 1) {
    selectorPage.value--
    await fetchSelectorImages()
  }
}

const selectorNextPage = async () => {
  if (selectorPage.value < selectorTotalPages.value) {
    selectorPage.value++
    await fetchSelectorImages()
  }
}

// Initial view Photo Sphere Viewer setup
const openInitialViewModal = async (pano) => {
  activePano.value = pano
  showInitialViewModal.value = true
  await nextTick()
  initViewer(pano)
}

const initViewer = (pano) => {
  if (viewerInstance) {
    viewerInstance.destroy()
    viewerInstance = null
  }

  const yaw = parseFloat(pano.initialYaw) || 0
  const pitch = parseFloat(pano.initialPitch) || 0
  const fov = parseFloat(pano.initialFov) || 90

  viewerInstance = new Viewer({
    container: viewerContainer.value,
    panorama: pano.panoramaImageUrl,
    caption: pano.title,
    defaultYaw: yaw,
    defaultPitch: pitch,
    defaultZoomLvl: fovToZoomLevel(fov),
    navbar: [
      'zoom',
      'caption',
      'fullscreen'
    ]
  })
}

const saveInitialView = async () => {
  if (!viewerInstance || !activePano.value) return

  savingInitialView.value = true
  try {
    const position = viewerInstance.getPosition()
    const zoomLevel = viewerInstance.getZoomLevel()
    
    // Get actual fov if available, or fall back to calculation
    const fov = viewerInstance.state?.fov || zoomLevelToFov(zoomLevel)

    const response = await api.patch(`/api/admin/panoramas/${activePano.value.id}/initial-view`, {
      yaw: position.yaw,
      pitch: position.pitch,
      fov: fov
    })

    if (response.success) {
      showInitialViewModal.value = false
      if (viewerInstance) {
        viewerInstance.destroy()
        viewerInstance = null
      }
      toastStore.success('Đã cập nhật góc nhìn ban đầu.')
      await fetchPanoramas() // reload details to show new coordinates
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi khi lưu góc nhìn ban đầu.')
  } finally {
    savingInitialView.value = false
  }
}

const closeInitialViewModal = () => {
  if (viewerInstance) {
    viewerInstance.destroy()
    viewerInstance = null
  }
  showInitialViewModal.value = false
}

onMounted(() => {
  fetchPanoramas()
})
</script>
