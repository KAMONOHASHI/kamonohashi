export type DeepWriteable<T> = {
  -readonly [P in keyof T]: DeepWriteable<T[P]>
}

import Vue from 'vue'
declare module 'vue/types/vue' {
  interface Vue {
    showSuccessMessage(): void
  }
}

import { ExtendedVue, VueConstructor } from 'vue'
type ComponentMethods<Comp> = Comp extends ExtendedVue<
  Vue,
  unknown,
  infer I,
  unknown,
  unknown
>
  ? I
  : never
export type TypedVueRef<Comp extends VueConstructor> = Vue &
  ComponentMethods<Comp>
